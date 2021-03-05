using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using KillingZ.LEntity;

namespace KillingZ.GFX
{
    static class Assets
    {
        public static ContentManager content;
        private static Handler handler;

        //Tilles
        public static Sprite tile_empty;
        public static Sprite tile_stone;
        public static Sprite tile_sand;
        public static Sprite tile_upGrass_sand;
        public static Sprite tile_downGrass_sand;
        public static Sprite tile_leftGrass_sand;
        public static Sprite tile_rightGrass_sand;
        public static Sprite tile_sand_leftUpGrass;
        public static Sprite tile_sand_rightUpGrass;
        public static Sprite tile_sand_leftDownGrass;
        public static Sprite tile_sand_rightDownGrass;

        //Guns
        public static Gun g_SIG;
        public static Gun g_MAGNUM;
        public static Gun g_COLT;
        public static Gun g_P90;
        public static Gun g_UZI;
        public static Gun g_SG553;
        public static Gun g_SHOTGUN_LONG;
        public static Gun g_SHOTGUN_SHORT;
        public static Gun g_M4A1;
        public static Gun g_AK47;
        public static Gun g_G36;
        public static Gun g_FN_FAL;
        public static Gun g_VECTOR;
        public static Gun g_SCAR;
        public static Gun g_DRAGUNOV;
        public static Gun g_SNIPER;
        public static Gun g_DSR;
        public static Gun g_RIFLE;

        public static Gun[] guns;

        //Zombies
        public static Zombie zombieDog;
        public static Zombie zombie1;
        public static Zombie zombie2;
        public static Zombie zombie3;
        public static Zombie zombie4;

        //Sprites
        public static Sprite[][][] s_playerSprites;
        public static Sprite[] s_bloodExplosion;
        public static Sprite s_bullet;

        //Animations
        public static Animation a_bloodSpirtHard1;
        public static Animation a_bloodSpirtHard2;
        public static Animation a_bloodSpirtMedium1;
        public static Animation a_bloodSpirtMedium2;
        public static Animation a_bloodSpirtLight1;
        public static Animation a_bloodSpirtLight2;

        //Texture
        public static Texture2D t_canvasBackground;

        //Fonts
        public static SpriteFont Arial13;
        public static SpriteFont Arial15;
        public static SpriteFont Arial18;


        public static void LoadContent()
        {
            LoadTextures();

            //Fonts
            Arial13 = content.Load<SpriteFont>("Fonts/Arial13");
            Arial15 = content.Load<SpriteFont>("Fonts/Arial15");
            Arial18 = content.Load<SpriteFont>("Fonts/Arial18");
        }

        public static void LoadGameContent()
        {
            LoadTiles();
            LoadGuns();
            LoadZombies();
            LoadSprites();
            LoadAnimations();
        }

        private static void LoadTiles()
        {
            //Tiles
            Texture2D tile_sheet = content.Load<Texture2D>("Textures/ground_tiles");

            tile_empty = new Sprite(tile_sheet, 0, 0, 32, 32);
            tile_stone = new Sprite(tile_sheet, 128, 96, 32, 32);
            tile_sand = new Sprite(tile_sheet, 32, 160, 32, 32);
            tile_upGrass_sand = new Sprite(tile_sheet, 224, 128, 32, 32);
            tile_leftGrass_sand = new Sprite(tile_sheet, 192, 160, 32, 32);
            tile_rightGrass_sand = new Sprite(tile_sheet, 256, 160, 32, 32);
            tile_downGrass_sand = new Sprite(tile_sheet, 224, 192, 32, 32);
            tile_sand_leftUpGrass = new Sprite(tile_sheet, 288, 192, 32, 32);
            tile_sand_rightUpGrass = new Sprite(tile_sheet, 320, 192, 32, 32);
            tile_sand_leftDownGrass = new Sprite(tile_sheet, 288, 160, 32, 32);
            tile_sand_rightDownGrass = new Sprite(tile_sheet, 320, 160, 32, 32);
        }

        private static void LoadGuns()
        {
            //Guns
            Texture2D gun_set = content.Load<Texture2D>("Textures/gun_set_custom");

            Sprite M4A1_sprite = new Sprite(gun_set, 10, 10, 98, 36);
            Sprite AK47_sprite = new Sprite(gun_set, 10, 130, 97, 31);
            Sprite G36_sprite = new Sprite(gun_set, 280, 10, 102, 35);
            Sprite SCAR_sprite = new Sprite(gun_set, 10, 90, 96, 33);
            Sprite FN_FAL_sprite = new Sprite(gun_set, 10, 250, 123, 27);
            Sprite VECTOR_sprite = new Sprite(gun_set, 10, 330, 77, 28);
            Sprite P90_sprite = new Sprite(gun_set, 280, 290, 62, 26);
            Sprite UZI_sprite = new Sprite(gun_set, 280, 370, 54, 33);
            Sprite SG553_sprite = new Sprite(gun_set, 10, 370, 85, 27);
            Sprite MAGNUM_sprite = new Sprite(gun_set, 10, 500, 56, 31);
            Sprite SIG_sprite = new Sprite(gun_set, 230, 460, 39, 28);
            Sprite COLT_sprite = new Sprite(gun_set, 360, 500, 35, 24);
            Sprite SHOTGUN_LONG_sprite = new Sprite(gun_set, 510, 250, 99, 22);
            Sprite SHOTGUN_SHORT_sprite = new Sprite(gun_set, 670, 250, 73, 21);
            Sprite DSR_sprite = new Sprite(gun_set, 510, 170, 125, 32);
            Sprite SNIPER_sprite = new Sprite(gun_set, 670, 130, 138, 35);
            Sprite RIFLE_sprite = new Sprite(gun_set, 670, 170, 142, 27);
            Sprite DRAGUNOV_sprite = new Sprite(gun_set, 510, 10, 139, 30);

            SoundEffect ak47_shot = content.Load<SoundEffect>("Sounds/ak47_shot");
            SoundEffect ak47_reload = content.Load<SoundEffect>("Sounds/ak47_reload");
            SoundEffect[] sounds = { ak47_shot, ak47_reload };

            SoundEffect[] m4a1_sfx =
            {
                content.Load<SoundEffect>("Sounds/m4a1_shot"),
                ak47_reload
            };

            SoundEffect[] fn_fal_sfx =
            {
                content.Load<SoundEffect>("Sounds/hard_shot"),
                ak47_reload
            };

            SoundEffect[] pistol_sfx =
            {
                content.Load<SoundEffect>("Sounds/small_pistol_shot"),
                ak47_reload
            };

            SoundEffect[] rifle_sfx =
            {
                content.Load<SoundEffect>("Sounds/rifle_shot"),
                content.Load<SoundEffect>("Sounds/rifle_reload")
            };

            SoundEffect[] shotgun_sfx =
            {
                content.Load<SoundEffect>("Sounds/shotgun_shot"),
                content.Load<SoundEffect>("Sounds/shotgun_fill")
            };

            float def_rs = 2.6f;
            float sg_rs = 0.8f;

            g_SIG = new Gun("Sig", SIG_sprite, pistol_sfx, 40, 15, 240, 200, Gun.Weights.LIGHT, Gun.Tier.Tier1, Gun.Type.Handlegun, 150, new Vector2(18, 20), def_rs, 20);
            g_MAGNUM = new Gun("Magnum", MAGNUM_sprite, fn_fal_sfx, 105, 6, 128, 100, Gun.Weights.HEAVY, Gun.Tier.Tier1, Gun.Type.Handlegun, 450, new Vector2(15, 20), def_rs, 20);
            g_COLT = new Gun("Colt", COLT_sprite, sounds, 82, 12, 144, 200, Gun.Weights.LIGHT, Gun.Tier.Tier1, Gun.Type.Handlegun, 500, new Vector2(18, 20), def_rs, 20);

            g_SG553 = new Gun("SG553", SG553_sprite, m4a1_sfx, 30, 30, 300, 700, Gun.Weights.MEDIUM, Gun.Tier.Tier2, Gun.Type.SMG, 400, new Vector2(5, 20), def_rs, 20);
            g_P90 = new Gun("P90", P90_sprite, sounds, 25, 50, 300, 900, Gun.Weights.MEDIUM, Gun.Tier.Tier2, Gun.Type.SMG, 450, new Vector2(10, 20), def_rs, 20);
            g_UZI = new Gun("UZI", UZI_sprite, sounds, 30, 40, 300, 600, Gun.Weights.MEDIUM, Gun.Tier.Tier2, Gun.Type.SMG, 500, new Vector2(10, 20), def_rs, 20);
            g_VECTOR = new Gun("VECTOR", VECTOR_sprite, sounds, 30, 25, 300, 1300, Gun.Weights.MEDIUM, Gun.Tier.Tier2, Gun.Type.SMG, 2500, new Vector2(-2, 20), def_rs, 20);
            g_SHOTGUN_SHORT = new Gun("Short shotgun", SHOTGUN_SHORT_sprite, shotgun_sfx, 200, 8, 96, 60, Gun.Weights.HEAVY, Gun.Tier.Tier2, Gun.Type.Shotgun, 500, new Vector2(10, 22), sg_rs, 20);
            g_SHOTGUN_LONG = new  Gun("Long shotgun", SHOTGUN_LONG_sprite, shotgun_sfx, 270, 10, 100, 55, Gun.Weights.HEAVY, Gun.Tier.Tier2, Gun.Type.Shotgun, 750, new Vector2(-2, 22), sg_rs, 20);

            g_G36 = new Gun("G36", G36_sprite, sounds, 40, 30, 300, 750, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 900, new Vector2(-2, 20), def_rs, 20);
            g_M4A1 = new Gun("M4A1", M4A1_sprite, m4a1_sfx, 35, 30, 300, 800, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 1000, new Vector2(-2, 20), def_rs, 20);
            g_AK47 = new Gun("AK47", AK47_sprite, sounds, 45, 30, 300, 600, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 1000, new Vector2(-2, 20), def_rs, 20);
            g_SCAR = new Gun("SCAR", SCAR_sprite, fn_fal_sfx, 60, 20, 300, 625, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 2500, new Vector2(-2, 20), def_rs, 20);
            g_FN_FAL = new Gun("FN_FAL", FN_FAL_sprite, fn_fal_sfx, 65, 20, 300, 650, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 2750, new Vector2(-2, 20), def_rs, 20);

            g_RIFLE = new Gun("Rifle", RIFLE_sprite, rifle_sfx, 300, 1, 100, 60, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 600, new Vector2(-2, 20), def_rs, 20);
            g_SNIPER = new Gun("Sniper", SNIPER_sprite, m4a1_sfx, 300, 10, 100, 60, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 1500, new Vector2(-2, 20), def_rs, 20);
            g_DRAGUNOV = new Gun("Dragunov", DRAGUNOV_sprite, m4a1_sfx, 400, 10, 100, 60, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 2000, new Vector2(-2, 20), def_rs, 20);
            g_DSR = new Gun("DSR", DSR_sprite, m4a1_sfx, 300, 10, 100, 200, Gun.Weights.HEAVY, Gun.Tier.Tier3, Gun.Type.Assault_Rifle, 2700, new Vector2(-2, 20), def_rs, 20);

            guns = new Gun[]{
                g_SIG, g_MAGNUM, g_COLT, g_P90, g_UZI, g_SG553, g_SHOTGUN_LONG, g_SHOTGUN_SHORT,
                g_M4A1, g_AK47, g_G36, g_FN_FAL, g_FN_FAL, g_VECTOR, g_SCAR, g_DRAGUNOV, g_SNIPER,
                g_DSR, g_RIFLE
            };
        }

        private static void LoadZombies()
        {
            //Zombies
            Texture2D zombie_sheet = content.Load<Texture2D>("Textures/zombie_sheet");
            Texture2D zombie_dog_sheet = content.Load<Texture2D>("Textures/zombie_dog_sheet");

            Sprite[][] zombieDog_sprites = new Sprite[][]
            {
                SheetCrop(zombie_dog_sheet, 4, 9, 217, 60, 40),
                SheetCrop(zombie_dog_sheet, 4, 7, 344, 60, 40),
            };
            Sprite[][] zombie1_sprites = new Sprite[][]
            {
               SheetCrop(zombie_sheet, 3, 0, 32, 32, 32),
               SheetCrop(zombie_sheet, 3, 0, 64, 32, 32)
            };
            Sprite[][] zombie2_sprites = new Sprite[][]
            {
               SheetCrop(zombie_sheet, 3, 96, 32, 32, 32),
               SheetCrop(zombie_sheet, 3, 96, 64, 32, 32)
            };
            Sprite[][] zombie3_sprites = new Sprite[][]
            {
               SheetCrop(zombie_sheet, 3, 192, 160, 32, 32),
               SheetCrop(zombie_sheet, 3, 192, 192, 32, 32)
            };
            Sprite[][] zombie4_sprites = new Sprite[][]
            {
               SheetCrop(zombie_sheet, 3, 288, 32, 32, 32),
               SheetCrop(zombie_sheet, 3, 288, 64, 32, 32)
            };

            zombieDog = new Zombie(zombieDog_sprites, 70, 2, 2.3f, 25);
            zombie1 = new Zombie(zombie1_sprites, 80, 3, 1.5f, 20);
            zombie2 = new Zombie(zombie2_sprites, 120, 6, 1.2f, 30);
            zombie3 = new Zombie(zombie3_sprites, 525, 14, 1f,  40);
            zombie4 = new Zombie(zombie4_sprites, 600, 15, 0.7f, 50);
        }

        private static void LoadSprites()
        {
            //Player
            Texture2D player_sheet = content.Load<Texture2D>("Textures/player_sheet");
            Texture2D dwarf_sheet = content.Load<Texture2D>("Textures/dwarf_sheet");
            Texture2D spacece_sheet = content.Load<Texture2D>("Textures/spacece_sheet");

            Sprite[][] player1Sprites = new Sprite[][]
            {
                SheetCrop(player_sheet, 3, 0, 32, 32, 32),
                SheetCrop(player_sheet, 3, 0, 64, 32, 32)
            };

           Sprite[][] player2Sprites = new Sprite[][]
           {
                SheetCrop(dwarf_sheet, 3, 0, 160, 32, 32),
                SheetCrop(dwarf_sheet, 3, 0, 192, 32, 32)
           };

            Sprite[][] player3Sprites = new Sprite[][]
           {
                SheetCrop(spacece_sheet, 3, 288, 32, 32, 32),
                SheetCrop(spacece_sheet, 3, 288, 64, 32, 32)
           };

            s_playerSprites = new Sprite[][][]
            {
                player1Sprites,
                player2Sprites,
                player3Sprites
            };

            //Sprites
            Texture2D bloodEx_sheet = content.Load<Texture2D>("Textures/blood_explosion");
            s_bloodExplosion = SheetCrop(bloodEx_sheet, 16, 0, 0, 128, 128, 4);

            //Bullet
            Texture2D bulletTexture = new Texture2D(handler.GetGraphicsDevice(), Bullet.DEFAULT_WIDTH, Bullet.DEFAULT_HEIGHT);

            Color[] data = new Color[Bullet.DEFAULT_WIDTH * Bullet.DEFAULT_HEIGHT];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black;
            bulletTexture.SetData(data);

            s_bullet = new Sprite(bulletTexture, 0, 0, bulletTexture.Width, bulletTexture.Height);
        }

        private static void LoadAnimations()
        {
            Texture2D blood_spirt_sheet = content.Load<Texture2D>("Textures/blood_spirt_sheet");
            Sprite[] blood_spirt_hard1 = SheetCrop(blood_spirt_sheet, 12, 0, 240, 40, 40);
            Sprite[] blood_spirt_hard2 = SheetCrop(blood_spirt_sheet, 13, 0, 200, 40, 40);
            Sprite[] blood_spirt_medium1 = SheetCrop(blood_spirt_sheet, 6, 0, 80, 40, 40);
            Sprite[] blood_spirt_medium2 = SheetCrop(blood_spirt_sheet, 10, 0, 120, 40, 40);
            Sprite[] blood_spirt_light1 = SheetCrop(blood_spirt_sheet, 6, 0, 0, 40, 40);
            Sprite[] blood_spirt_light2 = SheetCrop(blood_spirt_sheet, 7, 0, 40, 40, 40);

            a_bloodSpirtHard1 = new Animation(blood_spirt_hard1, 20, false);
            a_bloodSpirtHard2 = new Animation(blood_spirt_hard2, 20, false);
            a_bloodSpirtMedium1 = new Animation(blood_spirt_medium1, 20, false);
            a_bloodSpirtMedium2 = new Animation(blood_spirt_medium2, 20, false);
            a_bloodSpirtLight1 = new Animation(blood_spirt_light1, 20, false);
            a_bloodSpirtLight2 = new Animation(blood_spirt_light2, 20, false);
        }

        private static void LoadTextures()
        {
            t_canvasBackground = content.Load<Texture2D>("Textures/menuBackground");
        }

        public static void UnloadContent()
        {
            content.Unload();
        }

        private static Sprite[] SheetCrop(Texture2D sheet, int count, int x, int y, int width, int height)
        {
            Sprite[] sprites = new Sprite[count];

            for (int i = 0; i < count; i++)
            {
                sprites[i] = new Sprite(sheet, x + i * width, y, width, height);
            }

            return sprites;
        }

        private static Sprite[] SheetCrop(Texture2D sheet, int count, int x, int y, int width, int height, int spritesPerRow)
        {
            Sprite[] sprites = new Sprite[count];
            int row = 0;
            for (int i = 0; i < count; i++)
            {
                sprites[i] = new Sprite(sheet, x + i * width, y + row * height, width, height);
                if (i + 1 != 1 && i + 1 % spritesPerRow == 0) row++;
            }

            return sprites;
        }

        public static void SetContentManager(ContentManager cm)
        {
            content = cm;
        }

        public static void SetHandler(Handler _handler)
        {
            handler = _handler;
        }
    }
}
