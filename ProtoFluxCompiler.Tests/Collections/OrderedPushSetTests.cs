namespace ProtoFluxCompiler.Collections.Generic;

public sealed class OrderedPushSetTests
{
    [Fact]
    public static void Ordered()
    {
        var set = new OrderedPushSet<int>();
        Assert.Equal(set.ToList(), []);
        set.Add(0);
        Assert.Equal(set.ToList(), [0]);
        set.Add(5);
        Assert.Equal(set.ToList(), [5, 0]);
        set.Add(2);
        Assert.Equal(set.ToList(), [2, 5, 0]);
    }
}