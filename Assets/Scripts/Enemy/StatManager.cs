using GameplayMechanics.Character;

public class StatManager
{
    public Stat Life;
    public Stat Armor;
    public Stat Speed;
    public Stat Damage;
    public Stat Bleed;
    public Stat TriggeredDistance;
    public Stat IdleRadius;
    public int Level;

    public StatManager()
    {
        Life = new Stat("Health", 100f);
        Armor = new Stat("Armor", 0f);
        Speed = new Stat("Speed", 1f);
        Damage = new Stat("Damage", 15f);
        Bleed = null;
        TriggeredDistance = new Stat("TriggeredDistance", 10f);
        IdleRadius = new Stat("IdleRadius", 3f);
        this.Level = 0;
    }
        
    public StatManager(float life, float armor, float speed, float damage, int level, float triggeredDistance, float idleRadius)
    {
        Life = new Stat("Life", life);
        Armor = new Stat("Armor", armor);
        Speed = new Stat("Speed", speed);
        Damage = new Stat("Damage", damage);
        Bleed = new Stat("Bleed", Damage.GetFlat() * 0.1f);
        TriggeredDistance = new Stat("TriggeredDistance", triggeredDistance);
        IdleRadius = new Stat("IdleRadius", idleRadius);
        this.Level = level;
    }
}