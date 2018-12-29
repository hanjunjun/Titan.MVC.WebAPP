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
    public class SysDeptSvc
    {
		private ModelRespositoryFactory<SysDept, Guid> _modeSvc;
        public SysDeptSvc(ModelRespositoryFactory<SysDept, Guid> modeSvc)
        {
            _modeSvc = modeSvc;
        }
		 /// <summary>
        /// 添加对象
        /// </summary>
        /// <returns></returns>
        public void AddModel(SysDept entity)
        {
             _modeSvc.Add(entity);
        }

		/// <summary>
        /// 修改对象
        /// </summary>
        /// <returns></returns>
        public void UpdateModel(SysDept entity)
        {
           _modeSvc.Save(entity);
        }

		/// <summary>
        /// 删除对象
        /// </summary>
        /// <returns></returns>
        public void DeleteModel(SysDept entity)
        {
              _modeSvc.Remove(entity);
        }

		/// <summary>
        /// 根据主键查询对象
        /// </summary>
        /// <returns></returns>
        public SysDept FindModelById(Guid id)
        {
            SysDept tav = _modeSvc.FindById(id);
            return tav;
        }

		/// <summary>
        /// 根据表达式查询集合
        /// </summary>
        /// <returns></returns>
        public List<SysDept> FindModelByValue(Expression<Func<SysDept, bool>> predicate)
        {       
            List<SysDept> tavList = _modeSvc.GetDatas(predicate).ToList();
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
        public List<SysDept> GetModelList(int pageIndex,int pageSize,out int rowCount, Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, string>> orderby1,Expression<Func<SysDept, string>> orderby2,bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindPagedList(pageIndex, pageSize, out rowCount, selector, orderby1,orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelList(Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, string>> orderby1,Expression<Func<SysDept, string>> orderby2, bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindList(selector, orderby1,orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelListT(int pageIndex,int pageSize,out int rowCount, Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, DateTime>> orderby1,Expression<Func<SysDept, DateTime>> orderby2,bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindPagedList(pageIndex, pageSize, out rowCount, selector, orderby1,orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelListT(int pageIndex, int pageSize, out int rowCount, Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, DateTime>> orderby1, Expression<Func<SysDept, string>> orderby2, bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindPagedListOrderBy(pageIndex, pageSize, out rowCount, selector, orderby1, orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelListT(Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, DateTime>> orderby1,Expression<Func<SysDept, DateTime>> orderby2, bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindList(selector, orderby1,orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelListOrderBy(Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, int>> orderby1, Expression<Func<SysDept, DateTime>> orderby2, bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindListOrderBy(selector, orderby1, orderby2, isAsc).ToList();
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
        public List<SysDept> GetModelListOrderBy(Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, DateTime>> orderby1, Expression<Func<SysDept, string>> orderby2, bool isAsc)
        {
            List<SysDept> tavList = _modeSvc.FindListOrderBy(selector, orderby1, orderby2, isAsc).ToList();
            return tavList;
        }


		/// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="selector">查询条件lambda</param>
        /// <returns></returns>
        public int DeleteList(Expression<Func<SysDept, bool>> selector)
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
        public int UpdateList(Expression<Func<SysDept, bool>> selector, Expression<Func<SysDept, SysDept>> updateselector)
        {
            int t = _modeSvc.UpdateList(selector, updateselector);
            return t;
        }
    }
}
  