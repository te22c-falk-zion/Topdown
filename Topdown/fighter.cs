using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Raylib_cs;

public class Fighter

{
    public string name;
    public string enemyName = "lost";
    public int _hp = 100;
    public int _mindamage = 10;
    public int _maxdamage = 20;
    public int damage;

    public Boolean attack;
    public Boolean counter;
    public Boolean countermiss;

    public void Attack(Fighter target, Fighter user)
    {
        damage = Random.Shared.Next(_mindamage, _maxdamage);
        target.Hurt(damage);
        attack = true;
        counter = false;
        countermiss = false;
    }
    public void Counter(Fighter target, Fighter user)
    {
        int dice = Random.Shared.Next(_mindamage,_maxdamage);
        if (dice <= 20)
        {
            int damage = Random.Shared.Next(_mindamage, _maxdamage);
            target.Hurt(damage);
            counter = true;
            countermiss = false;
            attack = false;
        }
        else if (dice > 21)
        {
            int damage = Random.Shared.Next(_mindamage, _maxdamage);
            user.Hurt(damage); 
            countermiss = true;
            counter = false;
            attack = false;
        }

    }

    public void Hurt(int amount)
    {
        _hp -= amount;
        if(_hp < 0)
        {
            _hp = 0;
        }

    }

}