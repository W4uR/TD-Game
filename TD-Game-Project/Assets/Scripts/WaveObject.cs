public class WaveObject
{
    byte enemyId;
    byte numberOfEnemies;
    byte spawnTime;
    byte spawnerIndex;

    public WaveObject(byte enemyId, byte numberOfEnemies, byte spawnTime, byte spawnerIndex)
    {
        this.EnemyId = enemyId;
        this.NumberOfEnemies = numberOfEnemies;
        this.SpawnTime = spawnTime;
        this.SpawnerIndex = spawnerIndex;
    }

    public byte[] ToBytes => new byte[] { EnemyId, NumberOfEnemies, SpawnTime, SpawnerIndex };
    public static int Size => 4;
    public override string ToString()
    {
        return "id:" + enemyId + " c:" + numberOfEnemies + " si:" + SpawnerIndex + " st:" + SpawnTime;
    }
    public byte EnemyId { get => enemyId; set => enemyId = value; }
    public byte NumberOfEnemies { get => numberOfEnemies; set => numberOfEnemies = value; }
    public byte SpawnTime { get => spawnTime; set => spawnTime = value; }
    public byte SpawnerIndex { get => spawnerIndex; set => spawnerIndex = value; }
}
