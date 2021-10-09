using System;
using Xunit;
using Moq;

namespace MoqTest
{
    public interface IMockTest
    {
        string M1(int arg);
        string M2(int arg);
        event EventHandler<MockEventArgs> E1;
        string P1 { get; set; }
    }

    public class MockData
    {
        public static string M1Retuen { get; set; } = "M1Retuen";
        public static string P1Retuen { get; set; } = "P1Retuen";

        public static string EventReturn { get; set; } = "EventRetuen";
    }

    public class MockEventArgs : EventArgs
    {
        public string Source { get; set; }
    }


    public class MoqTest
    {
        public IMockTest _MockInstance;
        public int _M2CallCount;
        public MockEventArgs _MockEventArgs;
        public MoqTest()
        {
            var mock = new Mock<IMockTest>();
            mock.Setup(x => x.M1(It.IsAny<int>())).Returns(MockData.M1Retuen);
            mock.Setup(x => x.M2(It.IsAny<int>())).Returns(MockData.M1Retuen).Callback(() => { _M2CallCount++; });
            mock.Setup(x => x.P1).Returns(MockData.P1Retuen);
            mock.Raise(x => x.E1 += _MockInstance_E1, this, new MockEventArgs() { Source = MockData.EventReturn }); ;
            _MockInstance = mock.Object;
        }


        //methed
        [Fact]
        public void TestMethed()
        {
            Assert.Equal(_MockInstance.M1(0), MockData.M1Retuen);
        }

        //property
        [Fact]
        public void TestProperty()
        {
            Assert.Equal(_MockInstance.P1, MockData.P1Retuen);
        }

        //event
        [Fact]
        public void TestEvent()
        {
            //怎么模拟事件触发，响应？
            //Assert.Equal(_MockEventArgs.Source, MockData.EventReturn);
            //Assert.Equal(_MockEventArgs.Source, "");
        }

        private void _MockInstance_E1(object sender, MockEventArgs e)
        {
            Assert.Equal("", MockData.EventReturn);
        }

        //callBack
        [Fact]
        public void TestCallBack()
        {
            int callcount = 10;
            for (int i = 0; i < callcount; i++)
            {
                _MockInstance.M2(0);
            }
            Assert.Equal(callcount, _M2CallCount);
        }
    }
}
