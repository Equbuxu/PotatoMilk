using NUnit.Framework;
using PotatoMilk.Helpers;
using SFML.Graphics;
using System;

namespace PotatoMilkTests
{
    [TestFixture]
    public class ColorHelperTests
    {
        private Color zeros = new Color(0, 0, 0, 0);
        private Color _255s = new Color(255, 255, 255, 255);
        private Color _127s = new Color(127, 127, 127, 127);

        [Test]
        public void FromFloatsCreatesValidColor()
        {
            Assert.AreEqual(zeros, ColorHelper.FromFloats(0, 0, 0, 0));
            Assert.AreEqual(zeros, ColorHelper.FromFloats(-1f, -1f, -1f, -1f));
            Assert.AreEqual(_255s, ColorHelper.FromFloats(1f, 1f, 1f, 1f));
            Assert.AreEqual(_255s, ColorHelper.FromFloats(10f, 32f, 1.1f, 5f));
            Assert.AreEqual(_127s, ColorHelper.FromFloats(0.50001f, 0.5f, 0.5f, 0.5f));
        }

        [Test]
        public void FromStringCreatesValidColor()
        {
            Assert.AreEqual(zeros, ColorHelper.FromString("#0000"));
            Assert.AreEqual(zeros, ColorHelper.FromString("0000"));
            Assert.AreEqual(_255s, ColorHelper.FromString("#fFf"));
            Assert.AreEqual(_255s, ColorHelper.FromString("#FFFF"));
            Assert.AreEqual(_255s, ColorHelper.FromString("FFFFFF"));
            Assert.AreEqual(_255s, ColorHelper.FromString("ffffffff"));
            Assert.AreEqual(_127s, ColorHelper.FromString("7f7f7f7f"));
            Assert.AreEqual(Color.Blue, ColorHelper.FromString("00F"));
            Assert.Throws<Exception>(() => ColorHelper.FromString("01234"));
            Assert.Throws<Exception>(() => ColorHelper.FromString("z"));
        }
    }
}
