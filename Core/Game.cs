using MEC;
using PluginAPI.Core;
using SwiftNPCs.Core.Pathing;
using System.Collections.Generic;

namespace SwiftTD.Core
{
    public class Game
    {
        public int Lives = 10;
        public int Intermission = 15;

        public readonly List<EnemyMarker> Enemies = [];

        public readonly Queue<Wave> WaveQueue = [];

        public bool Lost => Lives <= 0;

        public Game(params Wave[] waves)
        {
            foreach (Wave w in waves)
                AddWave(w);
        }

        public void EnemyPassed(int lives)
        {
            Lives -= lives;

            Server.SendBroadcast("Leaked Enemy! \nLives Remaining: " + Lives, 3, shouldClearPrevious: true);

            if (Lost)
                Lose();
        }

        public void Lose()
        {
            WaveQueue.Clear();
            Server.SendBroadcast("Defense Failed! ", 5, shouldClearPrevious: true);
        }

        public void AddWave(Wave w)
        {
            WaveQueue.Enqueue(w);
        }

        public Wave SpawnNextWave(Path p)
        {
            Wave w = WaveQueue.Dequeue();
            w.SpawnAll(p, this);

            Server.SendBroadcast("Beginning Next Wave! \nRemaining " + WaveQueue.Count + " Waves.", 5, shouldClearPrevious: true);

            return w;
        }

        public void SpawnWaves(Path p)
        {
            Timing.RunCoroutine(Spawn(p));
        }

        public IEnumerator<float> Spawn(Path p)
        {
            while (WaveQueue.Count > 0)
            {
                bool finished = false;

                Wave w = SpawnNextWave(p);

                w.FinishCallback -= Finish;
                w.FinishCallback += Finish;

                while (!finished || Enemies.Count > 0)
                    yield return Timing.WaitForOneFrame;

                if (!Lost)
                {
                    if (WaveQueue.Count > 0)
                    {
                        Server.SendBroadcast("Wave Complete! \nNext Wave In " + Intermission + " Seconds.", 5, shouldClearPrevious: true);

                        yield return Timing.WaitForSeconds(Intermission);
                    }
                    else
                        Server.SendBroadcast("Completed All Waves! \nDefense Successful!", 5, shouldClearPrevious: true);
                }

                void Finish()
                {
                    finished = true;
                    w.FinishCallback -= Finish;
                }
            }
        }
    }
}
