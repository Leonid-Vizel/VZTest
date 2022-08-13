namespace VZTest.Instruments
{
    public static class ArrayTransformer
    {
        public static int[] ToIntArray(string value)
        {
            string[] stringArray = value.Split('-');
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
