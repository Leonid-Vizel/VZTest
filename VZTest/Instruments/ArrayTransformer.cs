namespace VZTest.Instruments
{
    public static class ArrayTransformer
    {
        public static int[] ToIntArray(string? value, char separator)
        {
            if (value == null)
            {
                return new int[0];
            }
            string[] stringArray = value.Split(separator);
            List<int> intAnswers = new List<int>();
            foreach(string element in stringArray)
            {
                if (int.TryParse(element, out int intElement))
                {
                    intAnswers.Add(intElement);
                }
            }
            return intAnswers.ToArray();
        }
    }
}
