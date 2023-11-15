using Raylib_cs;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

Random generator = new Random();

// List<string> names = new List<string>() {"Blah", "BlahBlah", "BlahBlahBlah", "Yip"};
// names.Add("Yap");
// names.Add("Yop");

// foreach (string name in names)
// {
//     Console.WriteLine(name);
// }
//  List<string> walls = new List<string>();
//  walls.Add(Rectangle(300, 200, 50, 100));

Raylib.InitWindow(800, 600, "Wsg gang :33");
Raylib.SetTargetFPS(60);

Color BG = new Color(58,58,58,255);
Color BLOOD = new Color(136, 8, 8, 255);

Rectangle characterRect = new Rectangle(300, 400, 64, 64);
Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");

List<Rectangle> walls = new();
walls.Add(new Rectangle(32, 32, 32, 256));
walls.Add(new Rectangle(64, 32, 128, 32));
walls.Add(new Rectangle(192, 32, 32, 128));
walls.Add(new Rectangle(250, 350, 32, 600));
walls.Add(new Rectangle(250,350,200,32));
walls.Add(new Rectangle(450,150, 32,232));

Vector2 movement = new Vector2(0.1f, 0.1f);

Vector2 size = new Vector2(50, 50);
Rectangle goal = new Rectangle(100, 100, 50, 50);
Rectangle point = new Rectangle(125,500,15,15);


string scene = "start";
int points = 0;
float speed = 5;

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
    characterRect.y += movement.Y;

    while (characterRect.x <= 0 || characterRect.x > 800-64)
    {
        characterRect.x -= movement.X;
    }

    while (characterRect.y <= 0)
    {
        characterRect.y = -movement.Y;
    }
    while (characterRect.y >= 600 - 64)
    {
        characterRect.y -= movement.Y;
    }

int[,] mapData = {
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,1,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
};

    Raylib.BeginDrawing();
    if (scene == "start")
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Press [SPACE] to start.", 120, 20, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
        {
            scene = "game";
            characterRect.x = 300;
            characterRect.y = 400;
        }
    }

    else if (scene == "game")
    {
        Raylib.ClearBackground(BG);
        Raylib.DrawRectangleRec(goal, BLOOD);
        if (points == 0)
        {
            Raylib.DrawRectangleRec(point,Color.GOLD);
            if (Raylib.CheckCollisionRecs(characterRect, point))
            {
                points =+ 1;
            }   
        }
        Raylib.DrawTexture(characterImage, (int)characterRect.x, (int)characterRect.y, Color.WHITE);
        Raylib.DrawText($"Points:{points}",0,0,25,Color.GOLD);
        for (int i = 0; i < walls.Count; i++)
        {
            Raylib.DrawRectangleRec(walls[i], Color.BLACK);
        }

        foreach (Rectangle wall in walls)
        {
            if (Raylib.CheckCollisionRecs(characterRect,wall))  
            {
                characterRect.x -= movement.X;
                if (movement.Y < 0)
                {characterRect.y -= movement.Y;}
                if (movement.Y > 0)
                {characterRect.y -= movement.Y;}
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