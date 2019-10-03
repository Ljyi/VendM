using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VendM.Core;
using VendM.Model.APIModelDto;
using VendM.Model.DataModel;
using VendM.Model.DataModel.Basics;
using VendM.Model.DataModel.Product;
namespace VendM.Service
{
    /// <summary>
    /// API
    /// </summary>
    public partial class ProductService
    {
        #region API
        /// <summary>
        /// 获取全部产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ProductAPIDto> GetAllProducts()
        {
            Expression<Func<Product, bool>> ex = t => true;
            ex = ex.And(t => !t.IsDelete);
            var productList = productRepository.GetEntities(ex);
            return Mapper.Map<List<Product>, List<ProductAPIDto>>(productList.ToList());
        }
        /// <summary>
        /// 获取机器详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MachineDetailAPIDto> GetMachineDetail(int machineId, int passageNumber = 0)
        {
            Expression<Func<MachineDetail, bool>> ex = t => true;
            if (machineId > 0)
                ex = ex.And(t => t.MachineId == machineId);
            if (passageNumber > 0)
                ex = ex.And(t => t.PassageNumber == passageNumber);
            ex = ex.And(t => !t.IsDelete);
            var machineDetailList = machinedetailRepository.GetEntities(ex);
            return Mapper.Map<List<MachineDetail>, List<MachineDetailAPIDto>>(machineDetailList.ToList());
        }
        /// <summary>
        /// 获取机器详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MachineAPIDto GetMachine(string machineno)
        {
            Expression<Func<Machine, bool>> ex = t => true;
            if (string.IsNullOrEmpty(machineno))
                return null;
            ex = ex.And(t => t.MachineNo == machineno);

            ex = ex.And(t => !t.IsDelete);
            var machine = machineRepository.GetEntities(ex).FirstOrDefault();
            machine.MachineDetails = machine.MachineDetails.Where(it => !it.IsDelete).ToList();
            return Mapper.Map<Machine, MachineAPIDto>(machine);
        }

        /// <summary>
        /// 获取机器详细信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<MachineStockDetails> GetMachineStockDetails(string machineno)
        {
            Expression<Func<MachineStockDetails, bool>> ex = t => true;
            if (string.IsNullOrEmpty(machineno))
                return null;
            ex = ex.And(t => t.MachineStock.Machine.MachineNo == machineno);
            ex = ex.And(t => !t.IsDelete);
            var machinestockDetail = machineStockDetailsRepository.GetEntities(ex).ToList();
            return machinestockDetail;
        }
        #endregion
    }
}
