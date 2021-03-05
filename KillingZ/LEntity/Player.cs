using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using KillingZ.GFX;

namespace KillingZ.LEntity
{
    class Player : Creature
    {
        public enum PlayerType { Type1, Type2, Type3, Type4};

        public int cash { get; private set; }
        public Gun currentGun { get; private set; }
        public Gun[] guns { get; private set; }

        private PlayerType type;

        //Keys
        private bool k_left;
        private bool k_right;
        private bool k_jump;
        private bool k_fire;
        private bool k_reload;
        private bool k_setGun1;
        private bool k_setGun2;
        private bool k_setGun3;
        private bool k_heal;
        private bool k_enchangeGun;

        //action - animations
        private static int IDLE = 0;
        private static int WALK_LEFT = 1;
        private static int WALK_RIGHT = 2;
        private static int currentAction;

        private Animation walk_left;
        private Animation walk_right;
        private Animation animation;

        //actions
        private bool healing;
        private float healingTimer;
        private float healingTime;

        private bool healingDelay;
        private float healingDelayTimer;
        private float healingDelayTime;

        private bool enchangingGun;
        private float enchangingGunTimer;
        private float enchangingGunTime;

        public Player(Handler handler, PlayerType type, Vector2 position): base(handler, position)
        {
            this.type = type;

            width = 32;
            height = 32;
            bounds = new Rectangle(0, 0, width, height);

            maxHealth = 100;
            health = maxHealth;
            speed = 2;
            jump_height = 10f;
            gravity = 0.6f;

            cash = 0;

            walk_left = new Animation(Assets.s_playerSprites[(int)type][0], 100, true);
            walk_right = new Animation(Assets.s_playerSprites[(int)type][1], 100, true);

            animation = walk_right;
            face_right = true;
            currentAction = IDLE;


            guns = new Gun[]
           {
                new Gun(Assets.g_SIG, handler, position, this, face_right),
                Gun.EMPTY,
                Gun.EMPTY
           };

            SetCurrentGun(guns[0]);


            healing = false;
            healingTimer = 0;
            healingTime = 2000;

            healingDelay = false;
            healingDelayTimer = 0;
            healingDelayTime = 8000;

            enchangingGun = false;
            enchangingGunTimer = 0;
            enchangingGunTime = 1000;
        }

        public override void Update(GameTime gameTime)
        {
            if (!alive) return;

            moveX = 0;
            moveY = 0;

            GetInput();

            if (healing)
            {
                healingTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (healingTimer >= healingTime)
                {
                    healingTimer = 0;
                    healing = false;
                    AddHealth(35);

                    healingDelay = true;
                }
            }

            if(healingDelay)
            {
                healingDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(healingDelayTimer > healingDelayTime)
                {
                    healingDelayTimer = 0;
                    healingDelay = false;
                }
            }

            if(enchangingGun)
            {
                enchangingGunTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(enchangingGunTimer > enchangingGunTime)
                {
                    enchangingGunTimer = 0;
                    enchangingGun = false;
                }
            }

            if (currentAction == IDLE || falling)
            {
                animation.SetActive(false);
            }
            else animation.SetActive(true);
            

            Move();
            animation.Update(gameTime);

            //Gun
            if (currentGun != Gun.EMPTY)
            {
                MoveWithGun();
                currentGun.Update(gameTime);

                if (currentGun.bulletsCount == 0 && currentGun.currentPlayerAmmo != 0 && !currentGun.IsReloading())
                {
                    currentGun.Reload();
                }

                if (k_fire && !healing)
                {
                    currentGun.Fire(gameTime);
                }
            }

            //camera
            handler.GetCamera().CenterOnEntity(this);
        }

        private void GetInput()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.D)) k_right = true;
            if (keyState.IsKeyDown(Keys.A)) k_left = true;
            if (keyState.IsKeyDown(Keys.W)) k_jump = true;
            if (keyState.IsKeyDown(Keys.Space)) k_fire = true;
            if (keyState.IsKeyDown(Keys.R)) k_reload = true;
            if (keyState.IsKeyDown(Keys.D1)) k_setGun1 = true;
            if (keyState.IsKeyDown(Keys.D2)) k_setGun2 = true;
            if (keyState.IsKeyDown(Keys.D3)) k_setGun3 = true;
            if (keyState.IsKeyDown(Keys.Q)) k_heal = true;
            if (keyState.IsKeyDown(Keys.E)) k_enchangeGun = true;

            if (keyState.IsKeyUp(Keys.D)) k_right = false;
            if (keyState.IsKeyUp(Keys.A)) k_left = false;
            if (keyState.IsKeyUp(Keys.W)) k_jump = false;
            if (keyState.IsKeyUp(Keys.Space)) k_fire = false;
            if (keyState.IsKeyUp(Keys.R)) k_reload = false;
            if (keyState.IsKeyUp(Keys.D1)) k_setGun1 = false;
            if (keyState.IsKeyUp(Keys.D2)) k_setGun2 = false;
            if (keyState.IsKeyUp(Keys.D3)) k_setGun3 = false;
            if (keyState.IsKeyUp(Keys.Q)) k_heal = false;
            if (keyState.IsKeyUp(Keys.E)) k_enchangeGun = false;

            //Move keys
            if (k_jump)
            {
                if (!falling)
                {
                    Jump();
                    animation.SetActive(false);
                }
            }

            if (k_right)
            {
                moveX = speed;

                if(currentAction != WALK_RIGHT)
                {
                    currentAction = WALK_RIGHT;
                    face_right = true;
                    animation = walk_right;
                    animation.SetActive(true);
                }
            }
            else if (k_left)
            {
                moveX = -speed;

                if(currentAction != WALK_LEFT)
                {
                    currentAction = WALK_LEFT;
                    face_right = false;
                    animation = walk_left;
                    animation.SetActive(true);
                }
            }
            else
            {
                if(currentAction != IDLE)
                {
                    currentAction = IDLE;
                }
            }

            //Actions
            if(k_reload)
            {
                if (currentGun.bulletsCount != currentGun.maxBulletsInStack && !healing)
                {
                    currentGun.Reload();
                }
            }

            if (k_heal)
            {
                if (!healing && health < 100 && !healingDelay)
                {
                    Heal();
                }
            }

            //Set gun keys
            if (k_setGun1)
            {
                if (currentGun != guns[0] && guns[0] != null) SetCurrentGun(guns[0]);
            }
            else if (k_setGun2)
            {
                if (currentGun != guns[1] && guns[1] != null) SetCurrentGun(guns[1]);
            }
            else if (k_setGun3)
            {
                if (currentGun != guns[2] && guns[2] != null) SetCurrentGun(guns[2]);
            }

            if(k_enchangeGun)
            {
                if(!enchangingGun && !healing)
                {
                    FindUnownedGun();
                }
            }
        }

        private void MoveWithGun()
        {
            if (face_right) currentGun.SetPosition(position.X + currentGun.offset.X, position.Y + currentGun.offset.Y);
            else currentGun.SetPosition(position.X - (currentGun.width + currentGun.offset.X - width), position.Y + currentGun.offset.Y);
        }

        protected override void Jump()
        {
            base.Jump();
            currentAction = IDLE;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if(health <= 0)
            {
                health = 0;
                alive = false;
                visible = false;
            }
        }

        public void AddHealth(int hp)
        {
            health += hp;
            if (health > maxHealth) health = maxHealth;
        }

        public void AddCash(int cashPoint)
        {
            cash += cashPoint;
        }

        //set current gun when switchim betwwen them
        private void SetCurrentGun(Gun gun)
        {
            if (currentGun != null && currentGun.IsReloading())
            {
                currentGun.StopReload();
            }

            currentGun = gun;
            MoveWithGun();
        }

        private void Heal()
        {
            if(currentGun != Gun.EMPTY && currentGun.IsReloading())
            {
                currentGun.StopReload();
            }

            healing = true;
        }

        //set gun at specific tier
        public void GetGun(Gun gun)
        {
            gun.SetOwner(this);

            guns[(int)gun.tier] = gun;

            if (currentGun == Gun.EMPTY) currentGun = gun;
            else if (currentGun.tier == gun.tier) currentGun = gun;
        }

        public void RemoveGun(int tier)
        {
            if (guns[tier] == currentGun) currentGun = Gun.EMPTY;
            guns[tier] = Gun.EMPTY;
        }

        private void FindUnownedGun()
        {
            for(int i = 0; i < handler.GetEntityManager().GetEntities().Count; i++)
            {
                Entity e = handler.GetEntityManager().GetEntities()[i];

                if(e is Gun)
                {
                    Gun gun = e as Gun;

                    if(gun.owner == Creature.NOBODY && Intersect(gun))
                    {
                        CollectGun(gun);
                        break;
                    }
                }
            }
        }

        private void CollectGun(Gun gun)
        {
            enchangingGun = true;

            handler.GetEntityManager().GetEntities().Remove(gun);
      
            DropGun((int)gun.tier, gun.position);
            GetGun(gun);
        }

        private void DropGun(int tier, Vector2 nPos)
        {
            if (guns[tier] == Gun.EMPTY) return;

            Gun g = guns[tier];

            g.SetOwner(Creature.NOBODY);
            g.SetPosition(nPos.X, nPos.Y);
            handler.GetEntityManager().GetEntities().Add(g);

            RemoveGun(tier);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            animation.GetSprite().Draw(sb, destRect);

            if (currentGun != Gun.EMPTY && !healing)
            {
                currentGun.Draw(sb);
            }
        }
    }
}
