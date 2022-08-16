using VZTest.Models.Test;

namespace VZTest.Instruments
{
    public static class Shuffler
    {
        public static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int i1 = random.Next(list.Count);
                int i2 = random.Next(list.Count);

                T temp = list[i1];
                list[i1] = list[i2];
                list[i2] = temp;
            }
        }
    }
}
