﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _04.TopDownAdventure
{
    public class SceneMap
    {
        Hero link;
        List<Projectile> projectiles = new List<Projectile>();
        float cooldownCounter = 0;
        const float COOLDOWN = 0.1f;

        Tileset tileset;
        Tilemap tilemap;

        List<Enemy> enemies;

        ContentManager content;

        public SceneMap(int[][] tilemapData, List<Enemy> enemies, string tilesetPath)
        {
            link = new Hero(100, 100, "hero");
            tileset = new Tileset(1, 3, 40, tilesetPath);
            tilemap = new Tilemap(tileset, tilemapData);

            this.enemies = enemies;
        }

        public void Load(ContentManager content)
        {
            this.content = content;

            link.Load(content);
            tilemap.Load(content);
            foreach (Enemy e in enemies)
            {
                e.Load(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            cooldownCounter += dt;

            // TODO: Add your update logic here
            link.Update(gameTime, tilemap);


            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Space) && cooldownCounter > COOLDOWN)
            {
                Projectile energyWave = new Projectile(link);
                energyWave.Load(content);
                energyWave.Visible = true;
                projectiles.Add(energyWave);
                cooldownCounter = 0;
            }

            foreach (Projectile p in projectiles)
            {
                p.Update(gameTime);
                foreach (Enemy e in enemies)
                {
                    if (CollisionSprites(e, p))
                    {
                        e.Visible = false;
                        p.Visible = false;
                    }
                }
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                if (!enemies[i].Visible)
                {
                    enemies.RemoveAt(i);
                }
            }
        }


        bool CollisionSprites(Sprite e, Sprite p)
        {
            // Si collision
            if (e.Rect.Intersects(p.Rect))
            {
                return true;
            }
            // sinon
            return false;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            tilemap.Draw(gameTime, spriteBatch);
            foreach (Enemy e in enemies)
            {
                e.Draw(gameTime, spriteBatch);
            }
            link.Draw(gameTime, spriteBatch);
            foreach (Projectile p in projectiles)
            {
                p.Draw(gameTime, spriteBatch);
            }
        }
    }
}
