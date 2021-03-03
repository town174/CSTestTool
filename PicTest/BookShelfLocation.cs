using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicTest
{
    /// <summary>
    /// 书架坐标(相对导航图)
    /// </summary>
    public class BookShelfLocation
    {
        /// <summary>
        /// 书架Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 书架名称
        /// </summary>
        public string BookShelfName { get; set; }

        /// <summary>
        /// 书架Id
        /// </summary>
        public string BookShelfId { get; set; }

        /// <summary>
        /// 是否A面
        /// </summary>
        public bool IsASide { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public int CoordinateX { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public int CoordinateY { get; set; }
    }
}
