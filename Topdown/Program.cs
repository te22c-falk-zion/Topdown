using Raylib_cs;
using System.ComponentModel;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks.Dataflow;

// random generator
Random generator = new Random();

// ints, bools, strings, floats
int screenWidth = 900;
int screenHeight = 650;
int gametime = 300;
int framerate = 60;
int airtime = framerate/3;

// bool jumping = false;
bool cameraBool = false;
bool text = false;

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

// Classes
Fighter enemy = new();
Fighter player = new();
Character character = new();

// Daring map hitboxes
character.MapHitboxes(walls, goals, points, pads, speeds,doubles,enemies,collidables,removables);


// Loop
while (!Raylib.WindowShouldClose())
{

    Raylib.BeginDrawing();

    Raylib.ClearBackground(Color.BLACK);

    // Controls scene
    if (character.scene == "controls")
    {
        Raylib.DrawText("Controls\n [W] to jump\n [A] to move left\n [D] to move right\n Shift to sprint\n press [Q] to move on", 80, 40, 30, BLOOD);
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
        {
            character.scene = "start";
        }
    }

    // Start scene
    else if (character.scene == "start")
    {
        // Dialogue
        Raylib.DrawText("Welcome oh honourless...The trials await you", 80, 40, 30, BLOOD);
        Raylib.DrawText("Press [Q] to begin your ordeal", 80, 100, 30, BLOOD);
        Raylib.DrawText("Collect points from enemies or scattered hearts", 80, 250, 30, BLOOD);

        // Starting the game
        if (Raylib.IsKeyPressed(KeyboardKey.KEY_Q))
        {
            character.scene = "game";
            cameraBool = true;
            character.scorePoints = 0;
            text = true;
        }
    }

    // inside the game
    else if (character.scene == "game")
    {
        // Simply targeting the camera onto the character
        if (cameraBool == true)
        {
            Raylib.BeginMode2D(camera);
            camera.target = new Vector2(character.characterRect.x + 32, character.characterRect.y + 32);
        }

        // Initialising methods.
        character.CheckCollision(points, speeds, enemies, goals);
        Raylib.ClearBackground(BG);
        character.Drawtime();
        character.MovementUpdate(doubles, walls, pads, collidables);
        character.CharImage();

        // Character score
        if (text == true)
        {
            Raylib.DrawText($"Points:{character.scorePoints}",(int)character.characterRect.x - 15,(int)character.characterRect.y - 50, 25, BLOOD);
        }
        
        Raylib.EndMode2D();
    }


    // Fight action
    else if (character.scene == "fight")
    {
        // Fight text
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("WEEWOO COMBAT!!!!", 80, 60, 40, BLOOD);
        Raylib.DrawText($"Your hp:{player._hp}",80, 500, 40, BLOOD);
        Raylib.DrawText($"Enemy hp:{enemy._hp}",600, 500, 40, BLOOD);

        Raylib.DrawText("ATTACK(A)                        COUNTER(G)", 20, 440, 40, BLOOD);

        // Fighting keys
        if(Raylib.IsKeyPressed(KeyboardKey.KEY_A))
        {
            player.Attack(enemy, player);
            enemy.Attack(player, enemy);
        }
        if(Raylib.IsKeyPressed(KeyboardKey.KEY_G))
        {
            player.Counter(enemy, player);
        }

        // Fight text
        if(player.attack == true)
        {
            Raylib.DrawText($"Player did {player.damage} to the enemy!", 80,100,40,Color.WHITE);
            Raylib.DrawText($"Enemy dealt {enemy.damage} to you!",80,140,40,Color.WHITE);
        }
        else if (player.counter == true)
        {
            Raylib.DrawText($"Succesful counter! You dealt {player.damage} damage!",80, 100, 40, Color.WHITE);
        }
        else if (player.countermiss == true)
        {
            Raylib.DrawText($"Unsuccesful couner.. You took {player.damage} damage", 80, 100, 40, Color.WHITE);
        }

        // If player ties
        if (enemy._hp <= 0 && player._hp <= 0)
        {
            character.scene = "game";
            enemy._hp = 100;
            player._hp = 100;
            player.counter = false;
            player.attack = false;
            player.countermiss = false;
        }
        // If player loses
        else if (player._hp <= 0)
        {
            character.scene = "GG";
        }
        // If player wins
        else if (enemy._hp <= 0)
        {
            character.scene = "game";
            character.scorePoints = character.scorePoints + 1;
            enemy._hp = 100;
            player._hp = 100;
            player.counter = false;
            player.attack = false;
            player.countermiss = false;
        }

    }

    // Won with 0 points dialogue
    else if (character.scene == "won" && character.scorePoints == 0)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("How did you?\nZero points?\nWow..you suck...", 120, 60, 40, BLOOD);
        if(gametime <= 0)
        {
        Raylib.DrawText("its enter to [esc] btw\nbet you suck too much to know that", 120, 240, 40, BLOOD);
        }
    }

    // Won with lower than 3 points dialogue
    else if (character.scene == "won" && character.scorePoints < 3)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Is that it?\nLeave at once.\nPress [esc] to leave", 120, 60, 40, BLOOD);
        gametime--;
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

    // Won with 6 points dialogue
    else if (character.scene == "won" && character.scorePoints == 6)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Marvelous\nSimply marvelous...\nPress [esc] to leave", 120, 60, 40, BLOOD);
    }
    else if (character.scene == "won" && character.scorePoints >= 3)
    {
        Raylib.ClearBackground(Color.BLACK);
        Raylib.DrawText("Good enough.\nThe trial is pleased.\nPress [esc] to leave", 120, 60, 40, BLOOD);
    }
    else if (character.scene == "GG")
    {
        Raylib.ClearBackground(BLOOD);
        Raylib.DrawText("You died, try to rely on the counter to not take damage", 120, 60, 20, Color.BLACK);
        Raylib.DrawText("it's risky but try and utilize it. [esc] to leave.", 140, 60, 20, Color.BLACK);
    }

    Raylib.EndDrawing();
}