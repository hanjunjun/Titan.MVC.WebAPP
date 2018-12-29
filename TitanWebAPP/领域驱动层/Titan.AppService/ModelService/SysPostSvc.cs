//------------------------------------------------------------------------------
//此代码由T4模板自动生成
//生成时间 2018-08-29 17:44:04
//对此文件的更改可能会导致不正确的行为，并且如果
//重新生成代码，这些更改将会丢失。
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Titan.Model;
using Titan.Model.DataModel;
using Titan.RepositoryCode;

namespace Titan.AppService.ModelService
{
    public class SysPostSvc
    {
		private ModelRespositoryFactory<SysPost, Guid> _modeSvc;
        public SysPostSvc(ModelRespositoryFactory<SysPost, Guid> modeSvc)
        {
            _modeSvc = modeSvc;
        }
		 /// <summary>
        /// 添加对象
        /// </summary>
        /// <returns></returns>
        public void AddModel(SysPost entity)
        {
             _modeSvc.Add(entity);
        }

		/// <summary>
        /// 修改对象
        /// </summary>
        /// <returns></returns>
        public void UpdateModel(SysPost entity)
        {
           _modeSvc.Save(entity);
        }

		/// <summary>
        /// 删除对象
        /// </summary>
        /// <returns></returns>
        public void DeleteModel(SysPost entity)
        {
              _modeSvc.Remove(entity);
        }

		/// <summary>
        /// 根据主键查询对象
        /// </summary>
        /// <returns></returns>
        public SysPost FindModelById(Guid id)
        {
            SysPost tav = _modeSvc.FindById(id);
            return tav;
        }

		/// <summary>
        /// 根据表达式查询集合
        /// </summary>
        /// <returns></returns>
        public List<SysPost> FindModelByValue(Expression<Func<SysPost, bool>> predicate)
        {       
            List<SysPost> tavList = _modeSvc.GetDatas(predicate).ToList();
            return tavList;
        }


		/// <summary>
        /// 根据根据条件查询并分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="rowCount">数据总数</param>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按string排序</param>
        /// <param name="orderby2">条件2：按string排序</param>
        /// <param name="isAsc">是否升序</param>
        public List<SysPost> GetModelList(int pageIndex,int pageSize,out int rowCount, Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, string>> orderby1,Expression<Func<SysPost, string>> orderby2,bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindPagedList(pageIndex, pageSize, out rowCount, selector, orderby1,orderby2, isAsc).ToList();
            return tavList;
        }

        /// <summary>
        /// 根据根据条件查询并分页
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按string排序</param>
        /// <param name="orderby2">条件2：按string排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelList(Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, string>> orderby1,Expression<Func<SysPost, string>> orderby2, bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindList(selector, orderby1,orderby2, isAsc).ToList();
            return tavList;
        }


	    /// <summary>
        /// 根据根据条件查询并分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="rowCount">数据总数</param>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按DateTime排序</param>
        /// <param name="orderby2">条件2：按DateTime排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelListT(int pageIndex,int pageSize,out int rowCount, Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, DateTime>> orderby1,Expression<Func<SysPost, DateTime>> orderby2,bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindPagedList(pageIndex, pageSize, out rowCount, selector, orderby1,orderby2, isAsc).ToList();
            return tavList;
        }

		/// <summary>
        /// 根据根据条件查询并分页
        /// </summary>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="rowCount">数据总数</param>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按DateTime排序</param>
        /// <param name="orderby2">条件2：按string排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelListT(int pageIndex, int pageSize, out int rowCount, Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, DateTime>> orderby1, Expression<Func<SysPost, string>> orderby2, bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindPagedListOrderBy(pageIndex, pageSize, out rowCount, selector, orderby1, orderby2, isAsc).ToList();
            return tavList;
        }

		/// <summary>
        /// 根据根据条件查询
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按DateTime排序</param>
        /// <param name="orderby2">条件2：按DateTime排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelListT(Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, DateTime>> orderby1,Expression<Func<SysPost, DateTime>> orderby2, bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindList(selector, orderby1,orderby2, isAsc).ToList();
            return tavList;
        }



		/// <summary>
        /// 根据根据条件查询(多组合查询)
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按int排序</param>
        /// <param name="orderby2">条件2：按DateTime排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelListOrderBy(Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, int>> orderby1, Expression<Func<SysPost, DateTime>> orderby2, bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindListOrderBy(selector, orderby1, orderby2, isAsc).ToList();
            return tavList;
        }


		/// <summary>
        /// 根据根据条件查询(多组合查询)
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="orderby1">条件1：按DateTime排序</param>
        /// <param name="orderby2">条件2：按string排序</param>
        /// <param name="isAsc">是否升序</param>
        /// <returns></returns>
        public List<SysPost> GetModelListOrderBy(Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, DateTime>> orderby1, Expression<Func<SysPost, string>> orderby2, bool isAsc)
        {
            List<SysPost> tavList = _modeSvc.FindListOrderBy(selector, orderby1, orderby2, isAsc).ToList();
            return tavList;
        }


		/// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <returns></returns>
        public int DeleteList(Expression<Func<SysPost, bool>> selector)
        {
            int t= _modeSvc.DeleteList(selector);
            return t;
        }


        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <param name="updateselector">更新对象</param>
        /// <returns></returns>
        public int UpdateList(Expression<Func<SysPost, bool>> selector, Expression<Func<SysPost, SysPost>> updateselector)
        {
            int t = _modeSvc.UpdateList(selector, updateselector);
            return t;
        }
    }
}
  