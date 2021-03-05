using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KillingZ.GFX;
using Microsoft.Xna.Framework.Audio;

namespace KillingZ.LEntity
{
    class Gun : Entity, ISellable
    {
        public static Gun EMPTY = null;

        public static Gun CreateGun(Gun prototype, Handler handler, Vector2 position, Creature owner, bool face_right)
        {
            switch (prototype.type)
            {
                case Type.Shotgun: return new Shotgun(prototype, handler, position, owner, face_right);
                default: return new Gun(prototype, handler, position, owner, face_right);
            }
        }

        public enum Weights { LIGHT, MEDIUM, HEAVY }
        public enum Tier { Tier1, Tier2, Tier3}
        public enum Type { Handlegun, SMG, Assault_Rifle, Shotgun}

        public int damage { get; protected set; }
        public float shotsPerMinute { get; protected set; }
        public int maxPlayerAmmo { get; protected set; } //max ammo of player
        public int currentPlayerAmmo { get; protected set; } //curent ammo of player
        public int maxBulletsInStack { get; private set; } //max bullets in one stack
        public int bulletsCount { get; protected set; } //current count of bullets in stack
        public Weights weight { get; protected set; }
        public Tier tier { get; protected set; }
        public Type type { get; protected set; }
        public Vector2 offset { get; protected set; }
        public float reloadSpeed { get; protected set; }
        public float bulletSpeed { get; protected set; }

        public Creature owner { get; protected set; }

        private int _cashValue;
        public int cashValue
        {
            get
            {
                return _cashValue;
            }
        }

        protected SoundEffect[] sounds;
        protected SoundEffectInstance reload_sfx;
        protected TimeSpan shotSpeed;
        protected TimeSpan previousShotTime;
        protected bool fire;
        protected bool reloading;
        protected bool reloadingFinished;
        protected float timer;
        protected float reloadSpeedMil;

        protected bool face_right;

        protected static int SHOT_SOUND = 0;
        protected static int RELOAD_SOUND = 1;

        //prototype
        public Gun(string name, Sprite texture, SoundEffect[] sounds, int damage, int maxBulletsInStack,
            int maxPlayerAmmo, float shotsPerMinute, Weights weight, Tier tier, Type type, int cashValue,
            Vector2 offset, float reloadSpeed, float bulletSpeed)
        {
            this.name = name;
            this.texture = texture;
            this.sounds = sounds;
            this.damage = damage;
            this.shotsPerMinute = shotsPerMinute;
            this.maxPlayerAmmo = maxPlayerAmmo;
            this.maxBulletsInStack = maxBulletsInStack;
            this.weight = weight;
            this.tier = tier;
            this.type = type;
            this._cashValue = cashValue;
            this.offset = offset;
            this.reloadSpeed = reloadSpeed;
            this.bulletSpeed = bulletSpeed;
        }

        public Gun(Gun prototype, Handler handler, Vector2 position, Creature owner, bool face_right) : base(handler, position)
        {
            InitByPrototype(prototype);

            this.width = texture.GetWidth() / 2;
            this.height = texture.GetHeight() / 2;

            this.owner = owner;
            this.face_right = face_right;

            bounds = new Rectangle(0, 0, width, height);

            currentPlayerAmmo = maxPlayerAmmo;
            bulletsCount = maxBulletsInStack;

            shotSpeed = TimeSpan.FromSeconds(60 / this.shotsPerMinute);
            previousShotTime = TimeSpan.Zero;
            fire = false;
            reloading = false;
            reloadingFinished = false;
            timer = 0;
            reloadSpeedMil = reloadSpeed * 1000;

            reload_sfx = sounds[RELOAD_SOUND].CreateInstance();
        }

        private void InitByPrototype(Gun prototype)
        {
            this.name = prototype.name;
            this.texture = prototype.texture;
            this.sounds = prototype.sounds;
            this.damage = prototype.damage;
            this.shotsPerMinute = prototype.shotsPerMinute;
            this.maxPlayerAmmo = prototype.maxPlayerAmmo;
            this.maxBulletsInStack = prototype.maxBulletsInStack;
            this.weight = prototype.weight;
            this.tier = prototype.tier;
            this.type = type;
            this._cashValue = prototype.cashValue;
            this.offset = prototype.offset;
            this.reloadSpeed = prototype.reloadSpeed;
            this.bulletSpeed = prototype.bulletSpeed;
        }


        public override void Update(GameTime gameTime)
        {
            if (owner != Creature.NOBODY)
            {
                face_right = owner.face_right;

                if (reloading)
                {
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                    if (timer >= reloadSpeedMil)
                    {
                        timer = 0;
                        reloadingFinished = true;
                        Reload();
                    }
                }

                if (fire)
                {
                    if (gameTime.TotalGameTime - previousShotTime > shotSpeed)
                    {
                        fire = false;
                    }
                }
            }
        }

        public virtual void Fire(GameTime gameTime)
        {
            if (reloading || bulletsCount <= 0) return;

            if(!fire)
            {
                sounds[SHOT_SOUND].Play();

                fire = true;
                CreateBullet();
                bulletsCount--;

                previousShotTime = gameTime.TotalGameTime;

                //if (bulletsCount == 0 && currentPlayerAmmo != 0) Reload();
            }
        }

        public virtual void Reload()
        {
            if(!reloading && !fire)
            {
                if (currentPlayerAmmo == 0) return;
                reload_sfx.Play();
                reloading = true;
                timer = 0;
            }

            if(reloadingFinished)
            {
                    reloading = false;
                    reloadingFinished = false;

                    int gotBullets = Math.Min(maxBulletsInStack - bulletsCount, currentPlayerAmmo);
                    bulletsCount += gotBullets;
                    currentPlayerAmmo -= gotBullets;
            }
        }

        private void CreateBullet()
        {
            Vector2 forward;
            Vector2 bulletPos;

            if (face_right)
            {
                forward = new Vector2(1, 0);
                bulletPos = new Vector2(position.X + width - Bullet.DEFAULT_WIDTH, position.Y + 3);
            }
            else
            {
                forward = new Vector2(-1, 0);
                bulletPos = new Vector2(position.X, position.Y + 3);
            }

            Bullet b = new Bullet(handler, bulletPos, forward, damage, bulletSpeed, weight);
            handler.GetEntityManager().Add(b);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            if(face_right)
            {
                texture.Draw(sb, destRect, SpriteEffects.None);
            }
            else
            {
                texture.Draw(sb, destRect, SpriteEffects.FlipHorizontally);
            }
        }

        public bool IsReloading()
        {
            return reloading;
        }

        public void StopReload()
        {
            reload_sfx.Stop();
            reloading = false;
        }

        public void SetPosition(float x, float y)
        {
            position = new Vector2(x, y);
        }

        public void SetOwner(Creature owner)
        {
            this.owner = owner;
        }
    }
}
