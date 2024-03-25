using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public class Fighter

{
    public int _hp = 100;
    public int _mindamage = 10;
    public int _maxdamage = 20;

    public void Attack(Fighter target)
    {
        int damage = Random.Shared.Next(_mindamage, _maxdamage);
        target.Hurt(damage);
    }
    public void Counter(Fighter target, Fighter user)
    {
        int dice = Random.Shared.Next(_mindamage,_maxdamage);
        if (dice <= 15)
        {
            int damage = Random.Shared.Next(_mindamage, _maxdamage) - 10;
            target.Hurt(damage);
        }
        else if (dice > 15)
        {
            int damage = Random.Shared.Next(_mindamage, _maxdamage);
            user.Hurt(damage); 
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