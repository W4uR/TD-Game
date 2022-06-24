public class WaveObject
{
    byte enemyId;
    byte numberOfEnemies;
    byte spawnTime;
    byte spawnerIndex;

    public WaveObject(byte enemyId, byte numberOfEnemies, byte spawnTime, byte spawnerIndex)
    {
        this.enemyId = enemyId;
        this.numberOfEnemies = numberOfEnemies;
        this.spawnTime = spawnTime;
        this.spawnerIndex = spawnerIndex;
    }

    public byte[] ToBytes => new byte[] { enemyId, numberOfEnemies, spawnTime, spawnerIndex };
    public byte SpawnerIndex => spawnerIndex;
    public static int Size => 4;
}
