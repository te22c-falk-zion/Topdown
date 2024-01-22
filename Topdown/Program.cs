using Raylib_cs;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

Random generator = new Random();

int screenWidth = 900;
int screenHeight = 650;
int charHeight = 64;
int charWidth = 54;
string scene = "start";
int ScorePoints = 0;
float speed = 8;
int tilesize = 64;
int pointsize = 16;
bool jumping = false;
float jump_speed = 25;
bool cameraBool = false;
bool text = false;
float charGravity = 7.0f;
bool Gravity = false;
int framerate = 60;
int airtime = framerate/3;
bool speeded = false;
int speedtime = 420;
int waittime = 180;
int doublesize = 32;

Raylib.InitWindow(screenWidth, screenHeight, "Wsg gang :33");
Raylib.SetTargetFPS(framerate);

Color BG = new Color(58, 58, 58, 255);
Color BLOOD = new Color(136, 8, 8, 255);

Rectangle characterRect = new Rectangle(320, 320, charWidth, charHeight);
Rectangle charfeet = new Rectangle(320, 384, charWidth, 8);
Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");
Texture2D Block = Raylib.LoadTexture("bwblock.png");
Texture2D Heart = Raylib.LoadTexture("heartPoint.png");
Texture2D Skull = Raylib.LoadTexture("skullGoal.png");
Texture2D brickBG = Raylib.LoadTexture("brickBG.png");
Texture2D jumppad = Raylib.LoadTexture("jump.png");
Texture2D speedb = Raylib.LoadTexture("speed.png");
Texture2D doubleJ = Raylib.LoadTexture("Up_arrow.png");
// List<Texture2D> textures = new();
// textures.Add(Raylib.LoadTexture("heartPoint.png"));


int[,] mapData = {
    {0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,3,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,5,0,0,3,0,2,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,1,1,1,1,1,1,4,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
};

List<Rectangle> walls = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 1)
            {
                Rectangle r = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                walls.Add(r);
            }
        }
    }
}
List<Rectangle> goals = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 2)
            {
                Rectangle g = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                goals.Add(g);
            }
        }
    }
}
List<Rectangle> points = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 3)
            {
                Rectangle p = new Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                points.Add(p);
            }
        }
    }
}
List<Rectangle> pads = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 4)
            {
                Rectangle b = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                pads.Add(b);
            }
        }
    }
}
List<Rectangle> speeds = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 5)
            {
                Rectangle s = new Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                speeds.Add(s);
            }
        }
    }
}
List<Rectangle> doubles = new();
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 6)
            {
                Rectangle d = new Rectangle(x * tilesize, y * tilesize, doublesize, doublesize);
                doubles.Add(d);
            }
        }
    }
}
Vector2 movement = new Vector2(0.1f, 0.1f);

Vector2 size = new Vector2(50, 50);
Rectangle goal = new Rectangle(100, 100, 50, 50);
Rectangle point = new Rectangle(125, 500, 15, 15);


Camera2D camera = new Camera2D();
camera.offset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
camera.rotation = 0.0f;
camera.zoom = 1.0f;


while (!Raylib.WindowShouldClose())
{
    movement = Vector2.Zero;

    bool grounded = isgrounded(charfeet, walls);
    // bool doubleCan = doublejump(characterRect, doubles);
    if (grounded == true){Gravity = false;}
    if (grounded == false){Gravity = true;}
    if (Gravity == true) {charGravity = 8.0f;}
    if (Gravity == false) {charGravity = 0;}
    // if (doubleCan == true){jumping = false;}


    Rectangle pointRect = CheckPointCollision(characterRect, points);
    if (pointRect.width != 0)
    {
        ScorePoints = ScorePoints + 1;
        points.Remove(pointRect);
    
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                if (mapData[(int)pointRect.y/tilesize,(int)pointRect.x/tilesize] == 3)
                {
                mapData[(int)pointRect.y/tilesize,(int)pointRect.x/tilesize] = 0;
                } 
            }
        }
    }
    Rectangle speedRect = CheckSpeedCollision(characterRect, speeds);
    if (speedRect.width != 0)
    {
        speeds.Remove(speedRect);
    
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                if (mapData[(int)speedRect.y/tilesize,(int)speedRect.x/tilesize] == 5)
                {
                mapData[(int)speedRect.y/tilesize,(int)speedRect.x/tilesize] = 0;
                speedtime =  420;
                speeded = true;
                }
            }
        }
    }
    if(speeded == true)
    {
        speed = 13;
        camera.zoom = 0.975f;
        speedtime--;
    }
    else if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_SHIFT))
    {
        speed = 11;
        camera.zoom = 0.98f;
    }
    else 
    {
        speed = 8;
        camera.zoom = 1.0f;
    }
    if (speedtime <= 0)
    {
        speeded = false;
        waittime--;
    }
    // if (Raylib.IsKeyDown(KeyboardKey.KEY_R))
    // {
    //     scene = "start";
    // }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {
        movement.X = -1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
        movement.X = 1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && jumping == false)
    {
        jumping = true;
        movement.Y = -1;
        Gravity = true;
    }
    if (airtime > 0 && jumping == true)
    {
        movement.Y = -1;
        jump_speed += -1f;
        airtime--;
    }
    else if (airtime <=0)
    {
        Gravity = true;
    }
    if (isgrounded(charfeet, walls))
    {
        jumping = false;
        Gravity = false;
        airtime = framerate/3;
        jump_speed = 28;
    }
    if (isboosted(charfeet, pads))
    {
        jumping = false;
        Gravity = false;
        airtime = framerate/3;
        jump_speed = 40;
    }
        if (jumping == true)
        {
            Console.WriteLine("jumping is true");
            
        }
        if (jumping == false)
        {
            Console.WriteLine("jumping is false");
            
        }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
    {
        movement.Y = 1;
    }

    // if (movement.Length() > 0)
    // {
    //     movement = Vector2.Normalize(movement);
    // }

    movement.X *= speed;

    characterRect.x += movement.X;
    charfeet.x += movement.X;
    // Kolla kollisioner
    if(CheckWallCollision(characterRect, walls, pads))
    {
        
        characterRect.x -= movement.X;
        charfeet.x -= movement.X;
    
    }

    characterRect.y += movement.Y * jump_speed + charGravity;
    // characterRect.y += charGravity;
    charfeet.y += movement.Y * jump_speed + charGravity;
    // charfeet.y += charGravity;
    // Kolla kollisioner
    if(CheckWallCollision(characterRect, walls, pads))
    {
        
        characterRect.y -= movement.Y * jump_speed + charGravity;
        // characterRect.y -= charGravity;
        charfeet.y -= movement.Y * jump_speed + charGravity;
        // charfeet.y -= charGravity;
    }
        

    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.BLACK);

    if (scene == "start")
    {
        Raylib.DrawText("Welcome oh honourless...The trials await you", 80, 40, 30, BLOOD);
        Raylib.DrawText("Press [space] to begin your ordeal", 120, 100, 30, BLOOD);
        Raylib.DrawText("Do your best to humour us...", 160, 350, 30, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            cameraBool = true;
            ScorePoints = 0;
            text = true;
            characterRect.x = 320;
            characterRect. y = 320;
            charfeet.x = 320;
            charfeet.y = 384;
        }
    }

    else if (scene == "game")
    {
        if (cameraBool == true)
        {
            Raylib.BeginMode2D(camera);
            camera.target = new Vector2(characterRect.x + 32, characterRect.y + 32);
        }
        Raylib.ClearBackground(BG);




        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                if (mapData[y, x] == 0)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Color.WHITE);
                }
                if (mapData[y, x] == 1)
                {
                    Raylib.DrawTexture(Block, x * tilesize, y * tilesize, Color.WHITE);
                }
                if (mapData[y, x] == 2)
                {
                    Raylib.DrawTexture(Skull, x * tilesize, y * tilesize, Color.WHITE);
                    
                }
                if (mapData[y, x] == 3)
                {
                    Raylib.DrawTexture(Heart,x * tilesize, y * tilesize,Color.WHITE);
                }
                if (mapData[y, x] == 4)
                {
                    Raylib.DrawTexture(jumppad,x * tilesize, y * tilesize,Color.WHITE);
                }
                if (mapData[y, x] == 5)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Color.WHITE);
                    Raylib.DrawTexture(speedb,x * tilesize, y * tilesize,Color.WHITE);
                }
                 if (mapData[y, x] == 6)
                {
                    Raylib.DrawTexture(brickBG, x * tilesize, y * tilesize, Color.WHITE);
                    Raylib.DrawTexture(doubleJ,x * tilesize, y * tilesize,Color.WHITE);
                }
            }
        }

        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Color.WHITE);
        Raylib.DrawRectangleRec(charfeet, color:Color.BLACK);

        
        if (text == true)
        {
            Raylib.DrawText($"Points:{ScorePoints}",(int)characterRect.x - 15,(int)characterRect.y - 50, 25, BLOOD);
        }
        
        Raylib.EndMode2D();
        foreach (Rectangle g in goals)
        {
            if(Raylib.CheckCollisionRecs(characterRect, g))
            {
                scene = "won";
            }
        }

    }

    else if (scene == "won")
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Good enough.\nThe trial is pleased.\nPress [ENTER] to leave", 120, 60, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
    }

    else if (scene == "defeat")
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Weak.\nPress [ENTER] to quit", 120, 60, 40, BLOOD);
        // gametime thing if they dont leave fast enough flash LEAVE and forcequit the window.
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        } 
    }

    Raylib.EndDrawing();
}

static bool CheckWallCollision(Rectangle characterRect, List<Rectangle> walls, List<Rectangle> pads)
{
    foreach (Rectangle r in walls)
    {
        if (Raylib.CheckCollisionRecs(characterRect, r))
        {
            return true;
        }
    }
    foreach (Rectangle b in pads)
    {
        if (Raylib.CheckCollisionRecs(characterRect, b))
        {
            return true;
        }
    }

    return false;
}

static Rectangle CheckPointCollision(Rectangle characterRect, List<Rectangle> points)
{
    foreach (Rectangle p in points)
    {
        if (Raylib.CheckCollisionRecs(characterRect, p))
        {
            return p;
        }
    }

    return new Rectangle();
}
static Rectangle CheckSpeedCollision(Rectangle characterRect, List<Rectangle> speeds)
{
    foreach (Rectangle s in speeds)
    {
        if (Raylib.CheckCollisionRecs(characterRect, s))
        {
            return s;
        }
    }

    return new Rectangle();
}

static bool isgrounded(Rectangle charfeet, List<Rectangle> walls)
{
    foreach (Rectangle r in walls)
    {
        if (Raylib.CheckCollisionRecs(charfeet, r))
        {
            return true;
        }
    }
    

    return false;
}
static bool isboosted(Rectangle charfeet, List<Rectangle> pads)
{
    foreach (Rectangle b in pads)
    {
        if (Raylib.CheckCollisionRecs(charfeet, b))
        {
            return true;
        }
    }
    return false;
}
// static bool doublejump(Rectangle characherRect, List<Rectangle> doubles)
// {
//     foreach (Rectangle d in doubles)
//     {
//         if (Raylib.CheckCollisionRecs(characherRect, d))
//         {
//             return true;
//         }
//     }
//     return false;
// }