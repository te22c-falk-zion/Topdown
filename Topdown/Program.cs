using Raylib_cs;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

Random generator = new Random();

// ints, bools, strings, floats
int screenWidth = 900;
int screenHeight = 650;
int charHeight = 64;
int charWidth = 54;
int speedtime = 420;
int waittime = 180;
int doublesize = 32;
int gametime = 300;
int restartY = 1088;
int tilesize = 64;
int pointsize = 16;
int ScorePoints = 0;
int framerate = 60;
int airtime = framerate/3;

bool jumping = false;
bool cameraBool = false;
bool text = false;
bool Gravity = false;
bool speeded = false;
bool candoublejump = false;

float charGravity = 7.0f;
float jump_speed = 25;
float speed = 8;
string scene = "start";

// Färger
Color BG = new Color(58, 58, 58, 255);
Color BLOOD = new Color(136, 8, 8, 255);

// Init
Raylib.InitWindow(screenWidth, screenHeight, "Wsg gang :33");
Raylib.SetTargetFPS(framerate);

// Camera init
Camera2D camera = new Camera2D();
camera.offset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
camera.rotation = 0.0f;
camera.zoom = 1.0f;

// Charactär
Rectangle characterRect = new Rectangle(448, 448, charWidth, charHeight);
Rectangle charfeet = new Rectangle(448, 512, charWidth, 8);

// Bilder
Texture2D characterImage = Raylib.LoadTexture("hollowhead.png");
Texture2D Block = Raylib.LoadTexture("bwblock.png");
Texture2D Heart = Raylib.LoadTexture("heartPoint.png");
Texture2D Skull = Raylib.LoadTexture("skullGoal.png");
Texture2D brickBG = Raylib.LoadTexture("brickBG.png");
Texture2D jumppad = Raylib.LoadTexture("jump.png");
Texture2D speedb = Raylib.LoadTexture("speed.png");
Texture2D doubleJ = Raylib.LoadTexture("Up_arrow.png");

// Map content
List<Rectangle> walls = new();
List<Rectangle> goals = new();
List<Rectangle> points = new();
List<Rectangle> pads = new();
List<Rectangle> speeds = new();
List<Rectangle> doubles = new();
List<Rectangle> enemies = new();
List<Rectangle> removables = new();
List<Rectangle> collidables = new();
List<Rectangle> effects = new();

// Map
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

// Map code
{
    for (int y = 0; y < mapData.GetLength(0); y++)
    {
        for (int x = 0; x < mapData.GetLength(1); x++)
        {
            if (mapData[y, x] == 1)
            {
                Rectangle r = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                walls.Add(r);
                collidables.Add(r);
            }
            if (mapData[y, x] == 2)
            {
                Rectangle g = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                goals.Add(g);
            }
            if (mapData[y, x] == 3)
            {
                Rectangle p = new Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                points.Add(p);
            }
            if (mapData[y, x] == 4)
            {
                Rectangle b = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                pads.Add(b);
                collidables.Add(b);
            }
            if (mapData[y, x] == 5)
            {
                Rectangle s = new Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                speeds.Add(s);
            }
            if (mapData[y, x] == 6)
            {
                Rectangle d = new Rectangle(x * tilesize, y * tilesize, pointsize, pointsize);
                doubles.Add(d);
            }
            if (mapData[y, x] == 7)
            {
                Rectangle e = new Rectangle(x * tilesize, y * tilesize, doublesize, doublesize);
                enemies.Add(e);
            }
        }
    }
}



while (!Raylib.WindowShouldClose())
{
    Vector2 movement = new Vector2(0.1f, 0.1f);
    movement = Vector2.Zero;

    bool grounded = FeetCollision(charfeet, walls);
    bool doubleCan = CheckWallCollision(characterRect, doubles);
    if (grounded == true){Gravity = false;}
    if (grounded == false){Gravity = true;}
    if (Gravity == true) {charGravity = 8.0f;}
    if (Gravity == false) {charGravity = 0;}
    if (doubleCan == true){candoublejump = true;}
    if (doubleCan == false){candoublejump = false;}


    Rectangle pointRect = CheckCollision(characterRect, points);
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



    Rectangle speedRect = CheckCollision(characterRect, speeds);
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

    
    if (Raylib.IsKeyDown(KeyboardKey.KEY_A) && scene == "game")
    {
        movement.X = -1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_D) && scene == "game")
    {
        movement.X = 1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_S) && scene == "game")
    {
        movement.Y = 1;
    }
    if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && jumping == false && scene == "game")
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
    if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && candoublejump == true)
    {
        movement.Y = -1;
        jump_speed = 28;
        jump_speed += -1f;
        airtime = framerate/3;
    }
    if (FeetCollision(charfeet, walls))
    {
        jumping = false;
        Gravity = false;
        airtime = framerate/3;
        jump_speed = 28;
    }
    if (FeetCollision(charfeet, pads))
    {
        jumping = false;
        Gravity = false;
        airtime = framerate/3;
        jump_speed = 40;
    }
    if (jumping == true)
    {
        Console.WriteLine($"jumping is true {characterRect.y}");
            
     }
    if (jumping == false)
    {
        Console.WriteLine($"jumping is false {characterRect.y}");
            
    }

    movement.X *= speed;

    characterRect.x += movement.X;
    charfeet.x += movement.X;

    characterRect.y += movement.Y * jump_speed + charGravity;
    charfeet.y += movement.Y * jump_speed + charGravity;

    // Retract movement into walls on the x axis
    if(CheckWallCollision(characterRect, collidables))
    {
        
        characterRect.x -= movement.X;
        charfeet.x -= movement.X;
    
    }

    // retract movement into walls on the y axis
    if(CheckWallCollision(characterRect, collidables))
    {
        
        characterRect.y -= movement.Y * jump_speed + charGravity;
        charfeet.y -= movement.Y * jump_speed + charGravity;
    }

    // If the chachater is below the map then reset their position
    if (characterRect.y >= restartY)
    {
        characterRect.x = 448;
        characterRect.y = 448;
        charfeet.x = 448;
        charfeet.y = 512;
    }
        

    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.BLACK);

    if (scene == "start")
    {
        Raylib.DrawText("Welcome oh honourless...The trials await you", 80, 40, 30, BLOOD);
        Raylib.DrawText("Press [Q] to begin your ordeal", 120, 100, 30, BLOOD);
        Raylib.DrawText("Do your best to humour us...", 160, 350, 30, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
        {
            scene = "game";
            cameraBool = true;
            ScorePoints = 0;
            text = true;
        }
    }

    else if (scene == "game")
    {
        // Simply targeting the camera onto the character
        if (cameraBool == true)
        {
            Raylib.BeginMode2D(camera);
            camera.target = new Vector2(characterRect.x + 32, characterRect.y + 32);
        }
        Raylib.ClearBackground(BG);

        //  Fetch the cordinates from the map data,
        for (int y = 0; y < mapData.GetLength(0); y++)
        {
            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                // then draw the image over that cordinate
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
                if (mapData[y, x] == 7)
                {
                    Raylib.DrawRectangle(x * tilesize, y*tilesize, tilesize, tilesize, Color.RED);
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
        foreach (Rectangle e in enemies)
        {
            if(Raylib.CheckCollisionRecs(characterRect,e))
            {
                scene = "combat";
            }
        }

    }

    else if (scene == "comabt")
    {

    }

    else if (scene == "won" && ScorePoints == 0)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("How did you?\nZero points?\nWow..you suck...", 120, 60, 40, BLOOD);
        if(gametime <= 0)
        {
        Raylib.DrawText("its enter to leave btw\nbet you suck too much to know that", 120, 240, 40, BLOOD);
        }
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
    }
    else if (scene == "won" && ScorePoints < 3)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Is that it?\nLeave at once.\nPress [ENTER] to leave", 120, 60, 40, BLOOD);
        gametime--;
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
        if (gametime <= 0)
        {
            Raylib.DrawText("I said leave.", 120, 220, 60, BLOOD);
        }
        if (gametime < -120)
        {
            Raylib.DrawText("Despicable creature.", 120, 300, 80, BLOOD);
        }
        if (gametime < -180)
        {
            Raylib.CloseWindow();
        }
    }
    else if (scene == "won" && ScorePoints == 6)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Marvelous\nSimply marvelous...\nPress [ENTER] to leave", 120, 60, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
    }
    else if (scene == "won" && ScorePoints >= 3)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Good enough.\nThe trial is pleased.\nPress [ENTER] to leave", 120, 60, 40, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
        {
            Raylib.CloseWindow();
        }
    }

    Raylib.EndDrawing();
}


// Checking collisions with items that are removed from the map
static Rectangle CheckCollision(Rectangle characterRect, List<Rectangle> removables)
{
    foreach (Rectangle p in removables)
    {
        if (Raylib.CheckCollisionRecs(characterRect, p))
        {
            return p;
        }
    foreach (Rectangle s in removables)
        if (Raylib.CheckCollisionRecs(characterRect, s))
        {
            return s;
        }
    }
    return new Rectangle();
}
// Checking collisions for items that the character has to collide into withour going through
static bool CheckWallCollision(Rectangle characterRect, List<Rectangle> collidables)
{
    foreach (Rectangle r in collidables)
    {
        if (Raylib.CheckCollisionRecs(characterRect, r))
        {
            return true;
        }
    }
    foreach (Rectangle b in collidables)
    {
        if (Raylib.CheckCollisionRecs(characterRect, b))
        {
            return true;
        }
    }
        foreach (Rectangle d in collidables)
    {
        if (Raylib.CheckCollisionRecs(characterRect, d))
        {
            return true;
        }
    }

    return false;
}
// Checking for if the character is grounded or is standing on a boost pad
static bool FeetCollision(Rectangle charfeet, List<Rectangle> effects)
{
    foreach (Rectangle r in effects)
    {
        if (Raylib.CheckCollisionRecs(charfeet, r))
        {
            return true;
        }
    }
    foreach (Rectangle b in effects)
    {
        if (Raylib.CheckCollisionRecs(charfeet, b))
        {
            return true;
        }
    }

    return false;
}
