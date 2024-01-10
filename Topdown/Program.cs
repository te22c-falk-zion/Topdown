using Raylib_cs;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

Random generator = new Random();

int screenWidth = 800;
int screenHeight = 600;
string scene = "start";
int ScorePoints = 0;
float speed = 5;
int tilesize = 64;
int pointsize = 16;
bool cameraBool = false;
bool text = false;

Raylib.InitWindow(screenWidth, screenHeight, "Wsg gang :33");
Raylib.SetTargetFPS(60);

Color BG = new Color(58, 58, 58, 255);
Color BLOOD = new Color(136, 8, 8, 255);

Rectangle characterRect = new Rectangle(300, 400, 64, 64);
Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");
Texture2D Block = Raylib.LoadTexture("bwblock.png");
Texture2D Heart = Raylib.LoadTexture("heartPoint.png");
Texture2D Skull = Raylib.LoadTexture("skullGoal.png");
// List<Texture2D> textures = new();
// textures.Add(Raylib.LoadTexture("heartPoint.png"));


int[,] mapData = {
    {0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
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

Vector2 movement = new Vector2(0.1f, 0.1f);

Vector2 size = new Vector2(50, 50);
Rectangle goal = new Rectangle(100, 100, 50, 50);
Rectangle point = new Rectangle(125, 500, 15, 15);


Camera2D camera = new Camera2D();
camera.target = new Vector2(characterRect.x + 20.0f, characterRect.y + 20.0f);
camera.offset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
camera.rotation = 0.0f;
camera.zoom = 1.0f;


while (!Raylib.WindowShouldClose())
{
    movement = Vector2.Zero;

    if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
    {
        movement.X = -1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
    {
        movement.X = 1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
    {
        movement.Y = -1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
    {
        movement.Y = 1;
    }

    if (movement.Length() > 0)
    {
        movement = Vector2.Normalize(movement);
    }

    movement *= speed;

    characterRect.x += movement.X;
    // Kolla kollisioner
    if(CheckWallCollision(characterRect, walls))
    {
        
        characterRect.x -= movement.X;
    
    }

    characterRect.y += movement.Y;
    // characterRect.y -= gravity;
    // Kolla kollisioner
    if(CheckWallCollision(characterRect, walls))
    {
        
        characterRect.y -= movement.Y;
        // characterRect.y += gravity;
    }

        Rectangle pointRect = CheckPointCollision(characterRect, points);
        if (pointRect.width != 0)
        {
            ScorePoints = ScorePoints + 1;
            points.Remove(pointRect);
        }
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
        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Color.WHITE);

    // while (characterRect.x <= 0 || characterRect.x > screenWidth-64)
    // {
    //     characterRect.x -= movement.X;
    // }

    // while (characterRect.y <= 0)
    // {
    //     characterRect.y = -movement.Y;
    // }
    // while (characterRect.y >= screenHeight - 64)
    // {
    //     characterRect.y -= movement.Y;
    // }


    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.BLACK);

    if (scene == "start")
    {
        Raylib.DrawText("Press [SPACE] to start.", 120, 20, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            cameraBool = true;
            ScorePoints = 0;
            characterRect.x = 300;
            characterRect.y = 400;
            text = true;
        }
    }

    else if (scene == "game")
    {
        Raylib.BeginMode2D(camera);
        if (cameraBool == true)
        {
            camera.target = new Vector2(characterRect.x +32 , characterRect.y + 32);
        }
        Raylib.ClearBackground(BG);

        // for (int i = 0; i < walls.Count; i++)
        // {
        //     Raylib.DrawRectangleRec(walls[i], Color.BLACK);
        // }

        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
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
            }
        }

        if (text == true)
        {
            Raylib.DrawText($"Points:{ScorePoints}",(int)camera.target.X - 336,(int)camera.target.Y - 236, 25, BLOOD);
        }

        foreach (Rectangle g in goals)
        {
            if(Raylib.CheckCollisionRecs(characterRect, g))
            {
                scene = "won";
            }
        }



        // Rectangle pointRect = CheckPointCollision(characterRect, points);
        // if (pointRect.width != 0)
        // {
        //     ScorePoints = ScorePoints + 1;
        //     points.Remove(pointRect);
        // }
        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Color.WHITE);


    }

    else if (scene == "won")
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("You win!\nPress [SPACE] to restart.\nPress [ENTER to close]", 120, 60, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            scene = "start";
            ScorePoints = 0;
        }
        else if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
    }

    Raylib.EndDrawing();
}

static bool CheckWallCollision(Rectangle characterRect, List<Rectangle> walls)
{
    foreach (Rectangle r in walls)
    {
        if (Raylib.CheckCollisionRecs(characterRect, r))
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