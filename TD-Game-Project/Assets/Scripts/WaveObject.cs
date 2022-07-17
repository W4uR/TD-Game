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
        this.SpawnerIndex1 = spawnerIndex;
    }



    public byte[] ToBytes => new byte[] { EnemyId, NumberOfEnemies, SpawnTime, SpawnerIndex1 };
    public byte SpawnerIndex => SpawnerIndex1;
    public static int Size => 4;

    public byte EnemyId { get => enemyId; set => enemyId = value; }
    public byte NumberOfEnemies { get => numberOfEnemies; set => numberOfEnemies = value; }
    public byte SpawnTime { get => spawnTime; set => spawnTime = value; }
    public byte SpawnerIndex1 { get => spawnerIndex; set => spawnerIndex = value; }
}
