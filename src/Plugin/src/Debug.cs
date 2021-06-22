namespace Meatyceiver2Plus
{
    public static class Debug
    {
        public static void ConsoleDebugging(short responseType, string failName, float rand, float percentChance)
        {
            if (!Plugin.Instance.EnableConsoleDebugging.Value) return;
            switch (responseType)
            {
                case 0:
                    UnityEngine.Debug.Log(failName + " RandomNum: " + rand + " to " + percentChance);
                    break;
                case 1:
                    UnityEngine.Debug.Log(failName + " failure!");
                    break;
            }
        }
    }
}