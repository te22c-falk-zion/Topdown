using System.Drawing;
using System.Numerics;
using Raylib_cs;

public class Character
{
    public int Height = 64;
    public int Width = 54;
    public int tilesize = 64;
    public int pointsize = 16;
    public int doublesize = 32;
    public int speedtime = 420;
    public int ScorePoints = 0;
    public int waittime = 180;
    public int airtime = 20;
    public int restartY = 1088;
    public float speed = 8;
    public float charGravity = 7.0f;
    public float jump_speed = 25;
    public bool speeded = false;
    public bool grounded = false;
    public bool doublecan = false;
    public bool Gravity = false;
    public bool jumping = false;
    public bool text = false;
    public string scene = "start";
    public Vector2 movement = new Vector2(0.1f, 0.1f);

    public Raylib_cs.Rectangle characterRect = new Raylib_cs.Rectangle(448, 448, 54, 64);
    public Raylib_cs.Rectangle charfeet = new Raylib_cs.Rectangle(448, 512, 54, 8);
    Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");
    Texture2D Block = Raylib.LoadTexture("bwblock.png");
    Texture2D Heart = Raylib.LoadTexture("heartPoint.png");
    Texture2D Skull = Raylib.LoadTexture("skullGoal.png");
    Texture2D brickBG = Raylib.LoadTexture("brickBG.png");
    Texture2D jumppad = Raylib.LoadTexture("jump.png");
    Texture2D speedb = Raylib.LoadTexture("speed.png");
    Texture2D doubleJ = Raylib.LoadTexture("Up_arrow.png");
    int[,] mapData = {
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {3,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,5,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,2,0},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,6,0,0,0,0,1,1,1,1,1,1,1,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,3,0,0,0,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,4,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,4,1,1,1,1,1,1,1,1,1,1,0,0,1,1,4,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,4,1,1,1,1,0,0,0,0,},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
};

    public void mapHitboxes(List<Raylib_cs.Rectangle> walls, List<Raylib_cs.Rectangle> goals, List<Raylib_cs.Rectangle> points, List<Raylib_cs.Rectangle> pads, List<Raylib_cs.Rectangle> speeds, List<Raylib_cs.Rectangle> doubles, List<Raylib_cs.Rectangle> enemies, List<Raylib_cs.Rectangle> collidables, List<Raylib_cs.Rectangle> removables)
    {
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                if (mapData[y, x] == 1)
                {
                    Raylib_cs.Rectangle r = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                    walls.Add(r);
                    collidables.Add(r);
                }
                if (mapData[y, x] == 2)
                {
                    Raylib_cs.Rectangle g = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                    goals.Add(g);
                }
                if (mapData[y, x] == 3)
                {
                    Raylib_cs.Rectangle p = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                    points.Add(p);
                }
                if (mapData[y, x] == 4)
                {
                    Raylib_cs.Rectangle b = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                    pads.Add(b);
                    collidables.Add(b);
                }
                if (mapData[y, x] == 5)
                {
                    Raylib_cs.Rectangle s = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                    speeds.Add(s);
                }
                if (mapData[y, x] == 6)
                {
                    Raylib_cs.Rectangle d = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, doublesize, doublesize);
                    doubles.Add(d);
                }
                if (mapData[y, x] == 7)
                {
                    Raylib_cs.Rectangle e = new Raylib_cs.Rectangle(x * tilesize, y * tilesize, doublesize, doublesize);
                    enemies.Add(e);
                }
            }
        }
    }
    public void drawtime()
    {
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                // then draw the image over that cordinate
                if (mapData[y, x] == 0)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 1)
                {
                    Raylib.DrawTexture(Block, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 2)
                {
                    Raylib.DrawTexture(Skull, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);

                }
                if (mapData[y, x] == 3)
                {
                    Raylib.DrawTexture(Heart, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 4)
                {
                    Raylib.DrawTexture(jumppad, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 5)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                    Raylib.DrawTexture(speedb, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 6)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                    Raylib.DrawTexture(doubleJ, x * tilesize, y * tilesize, Raylib_cs.Color.WHITE);
                }
                if (mapData[y, x] == 7)
                {
                    Raylib.DrawRectangle(x * tilesize, y * tilesize, tilesize, tilesize, Raylib_cs.Color.RED);
                }
            }
        }
    }

    public void CheckCollision(List<Raylib_cs.Rectangle> points, List<Raylib_cs.Rectangle> speeds, List<Raylib_cs.Rectangle> enemies, List<Raylib_cs.Rectangle> goals)
    {
        static Raylib_cs.Rectangle CheckCollision(Raylib_cs.Rectangle characterRect, List<Raylib_cs.Rectangle> removables)
        {
               foreach (Raylib_cs.Rectangle p in removables)
               {
                if (Raylib.CheckCollisionRecs(characterRect, p))
                {
                    return p;
                }
               }
                foreach (Raylib_cs.Rectangle s in removables)
                {
                    if (Raylib.CheckCollisionRecs(characterRect, s))
                    {
                        return s;
                    }
                }
                foreach (Raylib_cs.Rectangle e in removables)
                {
                    if (Raylib.CheckCollisionRecs(characterRect, e))
                    {
                        return e;
                    }
                }
                foreach (Raylib_cs.Rectangle g in removables)
                {
                    if (Raylib.CheckCollisionRecs(characterRect, g))
                    {
                        return g;
                    }
                }
            return new Raylib_cs.Rectangle();
        }
        Raylib_cs.Rectangle pointRect = CheckCollision(characterRect, points);
        if (pointRect.width != 0)
        {
            points.Remove(pointRect);
            ScorePoints = ScorePoints + 1;

            // for (int y = 0; y < mapData.GetLength(0); y++)
            // {
            //     for (int x = 0; x < mapData.GetLength(1); x++)
            //     {
                    // find the tile that is a pointrect then turn it to a 0s
                    if (mapData[(int)pointRect.y / tilesize, (int)pointRect.x / tilesize] == 3)
                    {
                        mapData[(int)pointRect.y / tilesize, (int)pointRect.x / tilesize] = 0;
                    }
            //     }
            // }
        }
        Raylib_cs.Rectangle speedRect = CheckCollision(characterRect, speeds);
        if (speedRect.width != 0)
        {
            speeds.Remove(speedRect);
            speedtime = 420;
            speeded = true;

            // for (int y = 0; y < mapData.GetLength(0); y++)
            // {
            //     for (int x = 0; x < mapData.GetLength(1); x++)
            //     {
                    // find the tile that is a speedrect then turn it to a 0s
                    if (mapData[(int)speedRect.y / doublesize, (int)speedRect.x / doublesize] == 5)
                    {
                        mapData[(int)speedRect.y / doublesize, (int)speedRect.x / doublesize] = 0;
                    }
            //     }
            // }
        }
        Raylib_cs.Rectangle goalRect = CheckCollision(characterRect, goals);
        if (goalRect.width != 0)
        {
            goals.Remove(goalRect);
            scene = "won";

            // for (int y = 0; y < mapData.GetLength(0); y++)
            // {
            //     for (int x = 0; x < mapData.GetLength(1); x++)
            //     {
                    // find the tile that is a pointrect then turn it to a 0s
                    if (mapData[(int)pointRect.y / tilesize, (int)pointRect.x / tilesize] == 2)
                    {
                        mapData[(int)pointRect.y / tilesize, (int)pointRect.x / tilesize] = 0;
                    }
            //     }
            // }
        }
        Raylib_cs.Rectangle enemiesRect = CheckCollision(characterRect, enemies);
        if (enemiesRect.width != 0)
        {
            enemies.Remove(enemiesRect);
            scene = "fight";
            // for (int y = 0; y < mapData.GetLength(0); y++)
            // {
            //     for (int x = 0; x < mapData.GetLength(1); x++)
            //     {

                    if (mapData[(int)speedRect.y / doublesize, (int)speedRect.x / doublesize] == 7)
                    {
                        mapData[(int)speedRect.y / doublesize, (int)speedRect.x / doublesize] = 0;
                    }
            //     }
            // }
        }


    }

    static bool CheckWallCollision(Raylib_cs.Rectangle characterRect, List<Raylib_cs.Rectangle> collidables)
    {
        foreach (Raylib_cs.Rectangle r in collidables)
        {
            if (Raylib.CheckCollisionRecs(characterRect, r))
            {
                return true;
            }
        }
        foreach (Raylib_cs.Rectangle b in collidables)
        {
            if (Raylib.CheckCollisionRecs(characterRect, b))
            {
                return true;
            }
        }
        foreach (Raylib_cs.Rectangle d in collidables)
        {
            if (Raylib.CheckCollisionRecs(characterRect, d))
            {
                return true;
            }
        }

        return false;
    }
    static bool FeetCollision(Raylib_cs.Rectangle charfeet, List<Raylib_cs.Rectangle> effects)
    {
        foreach (Raylib_cs.Rectangle r in effects)
        {
            if (Raylib_cs.Raylib.CheckCollisionRecs(charfeet, r))
            {
                return true;
            }
        }
        foreach (Raylib_cs.Rectangle b in effects)
        {
            if (Raylib.CheckCollisionRecs(charfeet, b))
            {
                return true;
            }
        }

        return false;
    }

    public void MovementUpdate(List<Raylib_cs.Rectangle> doubles, List<Raylib_cs.Rectangle> walls, List<Raylib_cs.Rectangle> pads, List<Raylib_cs.Rectangle> collidables)
    {
        movement = Vector2.Zero;
        // bool doublecan = CheckWallCollision(characterRect, doubles);
        bool grounded = FeetCollision(charfeet, walls);
        if (grounded == true) { Gravity = false; }
        if (grounded == false) { Gravity = true; }
        if (Gravity == true) { charGravity = 8.0f; }
        if (Gravity == false) { charGravity = 0; }

        if (speeded == true)
        {
            speed = 13;
            speedtime--;
        }
        else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
        {
            speed = 11;
        }
        else
        {
            speed = 8;
        }
        if (speedtime <= 0)
        {
            speeded = false;
            waittime--;
        }


        if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
        {
            movement.X = -1;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
        {
            movement.X += 1;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W) && jumping == false)
        {
            jumping = true;
            movement.Y = -1;
        }
        if (airtime > 0 && jumping == true)
        {
            movement.Y = -1;
            jump_speed += -1f;
            airtime--;
        }
        if (Raylib.IsKeyDown(KeyboardKey.KEY_W) && CheckWallCollision(characterRect, doubles) == true)
        {
            jump_speed = 28;
            jump_speed += -1f;
            airtime = 20;
        }
        // if feet are touching the gorund then 
        if (FeetCollision(charfeet, walls))
        {
            jumping = false;
            airtime = 20;
            jump_speed = 28;
        }
        if (FeetCollision(charfeet, pads))
        {
            jumping = false;
            airtime = 20;
            jump_speed = 40;
        }
        movement.X *= speed;

        characterRect.x += movement.X;
        charfeet.x += movement.X;

        characterRect.y += movement.Y * jump_speed + charGravity;
        charfeet.y += movement.Y * jump_speed + charGravity;

        if (CheckWallCollision(characterRect, collidables))
        {
            characterRect.x -= movement.X;
            charfeet.x -= movement.X;
        }
        if (characterRect.y >= restartY)
        {
            characterRect.x = 448;
            characterRect.y = 448;
            charfeet.x = 448;
            charfeet.y = 512;
        }

        if (CheckWallCollision(characterRect, collidables))
        {
            characterRect.y -= movement.Y * jump_speed + charGravity;
            charfeet.y -= movement.Y * jump_speed + charGravity;
        }

        if (text == true)
        {
            Raylib.DrawText($"Points:{ScorePoints}", (int)characterRect.x - 15, (int)characterRect.y - 50, 25, Raylib_cs.Color.RED);
        }


    }

    public void charImage()
    {
        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Raylib_cs.Color.WHITE);
    }

}


