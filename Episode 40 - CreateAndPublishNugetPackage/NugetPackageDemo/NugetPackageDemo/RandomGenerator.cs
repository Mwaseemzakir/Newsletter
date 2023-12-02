public static class RandomGenerator
{
	private static Random random = new Random();
	public static string RandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());
	}

	public static int RandomInteger(int min, int max)
	{
		return random.Next(min, max);
	}

	public static int RandomInteger()
	{
		return random.Next();
	}

}