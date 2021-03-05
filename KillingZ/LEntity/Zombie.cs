using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KillingZ.GFX;


namespace KillingZ.LEntity
{
    class Zombie : Creature
    {
        private Sprite[][] sprites;
        private int damage;
        private int cashPoint;

        private int dx;
        //private int dy;
        private bool attacking;
        private float attackTimer;
        private float jumpTimer;

        private static int IDLE = 0;
        private static int WALK_LEFT = 1;
        private static int WALK_RIGHT = 2;
        private int currentAction;

        private Animation walk_left;
        private Animation walk_right;
        private Animation deadAnim;
        private Animation currentAnimation;

        private Player player;

        //prototype
        public Zombie(Sprite[][] sprites, int health, int damage, float speed, int cashPoint)
        {
            this.sprites = sprites;
            this.damage = damage;
            this.speed = speed;
            this.maxHealth = health;
            this.cashPoint = cashPoint;
        }

        public Zombie(Zombie prototype, Handler handler, Vector2 position): base(handler, position)
        {
            this.sprites = prototype.sprites;
            this.damage = prototype.damage;
            this.speed = prototype.speed;
            this.maxHealth = prototype.maxHealth;
            this.health = maxHealth;
            this.cashPoint = prototype.cashPoint;

            width = 32;
            height = 32;
            bounds = new Rectangle(0, 0, width, height);
            attacking = false;

            attackTimer = 0;
            jumpTimer = 0;

            gravity = 0.6f;
            jump_height = 8;
            dx = 0;
            //dy = 0;

            float animSpeed = 300 / speed;
            walk_left = new Animation(sprites[WALK_LEFT - 1], animSpeed, true);
            walk_right = new Animation(sprites[WALK_RIGHT - 1], animSpeed, true);
            deadAnim = new Animation(Assets.s_bloodExplosion, 70, false);

            currentAnimation = walk_right;
            currentAction = WALK_RIGHT;

            player = handler.GetEntityManager().GetPlayer();
        }

        public override void Update(GameTime gameTime)
        {
            if (!alive)
            {
                //deadAnimation
                currentAnimation.Update(gameTime);
                if (!currentAnimation.IsActive()) visible = false;
                return;
            }

            moveX = 0;
            moveY = 0;

            //Attack time interval
            if(attacking)
            {
                attackTimer += gameTime.ElapsedGameTime.Milliseconds;
                if(attackTimer > 1000)
                {
                    attackTimer = 0;
                    attacking = false;
                }
            }

            //Move
            if (position.X >  player.position.X + player.width) dx = -1;
            if (position.X + width < player.position.X) dx = 1;

            if(dx > 0) //right
            {
                if(currentAction != WALK_RIGHT)
                {
                    currentAction = WALK_RIGHT;
                    currentAnimation = walk_right;
                    currentAnimation.SetActive(true);
                }

                moveX = speed * dx;
            }
            else if(dx < 0) //left
            {
                if (currentAction != WALK_LEFT)
                {
                    currentAction = WALK_LEFT;
                    currentAnimation = walk_left;
                    currentAnimation.SetActive(true);
                }

                moveX = speed * dx;
            }

            if (Intersect(player))
            {
                moveX = 0;
                currentAction = IDLE;
            }

            if(currentAction == IDLE)
            {
                currentAnimation.SetActive(false);
            }

            //Tille Collision -> Jump
            if (IsStucked() && !falling)
            {
                jumpTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (jumpTimer >= 700)
                {
                    jumpTimer = 0;
                    Jump();
                }
            }
            else jumpTimer = 0;

            Move();
            currentAnimation.Update(gameTime);

            CheckAttack();
        }

        private bool IsStucked()
        {
            int topTile = GetMapY(position.Y + moveY + bounds.Y);
            int bottomTile = GetMapY(position.Y + moveY + bounds.Y + bounds.Height - 1);
            int leftTile = GetMapY(position.X + moveX + bounds.X);
            int rightTile = GetMapY(position.X + moveX + bounds.X + bounds.Width);

            return TileCollision(leftTile, topTile) || TileCollision(leftTile, bottomTile) ||
                   TileCollision(rightTile, topTile) || TileCollision(rightTile, bottomTile);
        }

        public void CheckAttack()
        {
            List<Entity> entities = handler.GetEntityManager().GetEntities();
            for(int i = 0; i < entities.Count; i++)
            {
                Entity e = entities[i];
                if(e is Bullet && e.visible)
                {
                    Bullet b = (Bullet)e;
                    if(Intersect(b))
                    {
                        if(handler.GetRandom(1, 100) <= 40)
                        {
                            //Headshot
                            TakeDamage(b.damage + 100);
                            CreateBlood(ref b, Gun.Weights.HEAVY);
                        }
                        else
                        {
                            TakeDamage(b.damage);
                            CreateBlood(ref b, b.weight);
                        }

                        b.SetVisible(false);
                    }
                }

                if(e is Player && ((Creature)e).alive)
                {
                    if(Intersect(e))
                    {
                        Attack();
                    }
                }
            }
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if(health <= 0) //Dead
            {
                Dead();
            }
        }

        private void Dead()
        {
            health = 0;
            alive = false;
            player.AddCash(cashPoint);
            currentAnimation = deadAnim;

            handler.GetZombieWaveManager().DeadZombies(1);
            handler.SetKilledZombies(handler.GetKilledZombies() + 1);

            //Blood anim
            width = 50;
            height = 50;
            position = new Vector2(position.X - 9, position.Y - 9);

            //getGun
            if (handler.GetRandom(1, 100) <= 0)
            {
                DropItem();
            }
        }

        private void CreateBlood(ref Bullet b, Gun.Weights weight)
        {
            int x = b.forward.X > 0 ? (int)position.X + width / 2 : (int)position.X - width / 2;
            bool faceLeft = b.forward.X > 0 ? false : true;

            Animation blood;
            int r = handler.GetRandom(0, 1);

            switch(weight)
            {
                case Gun.Weights.LIGHT:
                    if(r == 0) blood = new Animation(Assets.a_bloodSpirtMedium1, handler, faceLeft);
                    else blood = new Animation(Assets.a_bloodSpirtMedium2, handler, faceLeft);
                    break;

                case Gun.Weights.MEDIUM:
                    if (r == 0) blood = new Animation(Assets.a_bloodSpirtMedium1, handler, faceLeft);
                    else blood = new Animation(Assets.a_bloodSpirtMedium2, handler, faceLeft);
                    break;

                case Gun.Weights.HEAVY:
                    if (r == 0) blood = new Animation(Assets.a_bloodSpirtHard1, handler, faceLeft);
                    else blood = new Animation(Assets.a_bloodSpirtHard2, handler, faceLeft);
                    break;

                default: blood = new Animation(Assets.a_bloodSpirtHard1, handler, faceLeft); break;
            }

            blood.SetRectangle(new Rectangle(x, (int)position.Y, 40, 40));

            handler.GetAnimationManager().Add(blood);
        }

        private void Attack()
        {
            if(!attacking)
            {
                attacking = true;
                player.TakeDamage(damage);
            }
        }

        private void DropItem()
        {
            int randomIndex = handler.GetRandom(0, Assets.guns.Length - 1);

            Gun g = Gun.CreateGun(Assets.guns[randomIndex], handler,  
                new Vector2(position.X, position.Y + width / 2), Creature.NOBODY, face_right);

            handler.GetEntityManager().Add(g);
        }


        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            currentAnimation.Draw(sb, destRect);
        }
    }
}
