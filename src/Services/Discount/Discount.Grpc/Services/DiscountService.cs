using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbcontext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupones.FirstOrDefaultAsync(x=>x.ProductName.ToLower() == request.ProductName.ToLower());
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name {request.ProductName} not found."));

            logger.LogInformation("Discount is retrieved for ProductName: {productName}, Amount : {amount}, Description : {description}", coupon.ProductName, coupon.Amount,coupon.Description);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>(); // coverting gRPC request model to DBcontext model,<Coupon> is from model class
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));

            dbcontext.Coupones.Add(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created, ProductName: {productName}, Amount : {amount}, Description : {description}", coupon.ProductName, coupon.Amount, coupon.Description);
            
            var couponModel = coupon.Adapt<CouponModel>(); // coverting back  DBcontext model to gRPC model,<CouponModel> is from Discount,proto class

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name {request.Coupon.ProductName} not found."));

            dbcontext.Coupones.Update(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation($"Discount is successfully updated, ProductName: {request.Coupon.ProductName}");

            var coupnModel = coupon.Adapt<CouponModel>();

            return coupnModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbcontext.Coupones.FirstOrDefaultAsync(x => x.ProductName.ToLower() == request.Coupon.ProductName.ToLower());
            if (coupon is null)
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with Product Name {request.Coupon.ProductName} not found."));

            dbcontext.Coupones.Remove(coupon);
            await dbcontext.SaveChangesAsync();

            logger.LogInformation($"Discount is successfully deleted, ProductName: {request.Coupon.ProductName}");

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
