using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ETS.Data.PageData
{
    public class PageInfo<T>
    {
        private int _Count;
        private int _Index;
        private IList<T> _dt;
        private int _pageSize;
        private int _totalPage;
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="Count">总记录数</param>
        /// <param name="dt">当前页的记录</param>
        public PageInfo(int Count, int index, IList<T> dt, int totalPage)
        {
            _dt = dt;
            if (_dt == null)
                _dt = new List<T>();
            _Count = Count;
            _Index = index;
            _totalPage = totalPage;
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int All
        {
            get
            {
                return _Count;
            }
        }
        /// <summary>
        /// 当前真实页号。
        /// </summary>
        public int Index
        {
            get
            {
                return _Index;
            }
        }
        /// <summary>
        /// 当前页的记录
        /// </summary>
        public IList<T> Records
        {
            get
            {
                return _dt;
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                //return _Count % _pageSize == 0 ? _Count / _pageSize : (_Count / _pageSize) + 1;
                return _totalPage;
            }
        }
    }
}
