using Raylib_cs;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

Random generator = new Random();

int screenWidth = 800;
int screenHeight = 600;
string scene = "start";
int points = 0;
float speed = 5;
int tilesize = 64;
bool cameraBool = false;
bool point1 = true;

Raylib.InitWindow(screenWidth, screenHeight, "Wsg gang :33");
Raylib.SetTargetFPS(60);

Color BG = new Color(58, 58, 58, 255);
Color BLOOD = new Color(136, 8, 8, 255);

Rectangle characterRect = new Rectangle(300, 400, 64, 64);
Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");

int[,] mapData = {
    {1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
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
    // Kolla kollisioner

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
            point1 = true;
            points = 0;
            characterRect.x = 300;
            characterRect.y = 400;
        }
    }

    else if (scene == "game")
    {
        Raylib.BeginMode2D(camera);
        if (cameraBool == true)
        {
            camera.target = new Vector2(characterRect.x + 32, characterRect.y + 32);
        }
        Raylib.ClearBackground(BG);

        if (point1 == true)
        {
            Raylib.DrawRectangleRec(point, Color.GOLD);
            if (Raylib.CheckCollisionRecs(characterRect, point))
            {
                points = +1;
                point1 = false;
            }
        }
        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Color.WHITE);
        Raylib.DrawText($"Points:{points}", 0, 0, 25, BLOOD);
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
                    Raylib.DrawRectangle(x * tilesize, y * tilesize, tilesize, tilesize, Color.BLACK);
                }
                if (mapData[y, x] == 2)
                {
                    Raylib.DrawRectangleRec(goal, BLOOD);
                }
                if (mapData[y, x] == 3)
                {
                    Raylib.DrawRectangleRec(point, Color.GOLD);
                }
            }
        }

        
        if (Raylib.CheckCollisionRecs(characterRect, goal))
        {
            scene = "won";
        }

    }

    else if (scene == "won")
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("You win!\nPress [SPACE] to restart.\nPress [ENTER to close]", 120, 60, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            scene = "start";
            points = 0;
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