using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Consts;
using ETicaretAPI.Application.CustomAttributies;
using ETicaretAPI.Application.Enum;
using ETicaretAPI.Application.Features.Commands.ProductImageFileSQRS.ChangeShowcaseImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFileSQRS.DeleteProductImage;
using ETicaretAPI.Application.Features.Commands.ProductImageFileSQRS.UploadProductImage;
using ETicaretAPI.Application.Features.Commands.ProductSQRS.CreateProduct;
using ETicaretAPI.Application.Features.Commands.ProductSQRS.DeleteProduct;
using ETicaretAPI.Application.Features.Commands.ProductSQRS.UpdateProduct;
using ETicaretAPI.Application.Features.Queries.ProductImageFileSQRS.GetProductImage;
using ETicaretAPI.Application.Features.Queries.ProductSQRS.GetAllProducts;
using ETicaretAPI.Application.Features.Queries.ProductSQRS.GetProductById;
using ETicaretAPI.Application.Repositories.File;
using ETicaretAPI.Application.Repositories.IncoiceFileT;
using ETicaretAPI.Application.Repositories.ProductIeFileT;
using ETicaretAPI.Application.Repositories.ProductImageFileT;
using ETicaretAPI.Application.Repositories.ProductT;
using ETicaretAPI.Application.RequestParamerts;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Domain.Entites;
using ETicaretAPI.Infrastructure.Services.Storage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepositpory _fileWriteRepositpory;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IStorageService _storageService;
        private readonly IConfiguration configuration;
        readonly IProductService _productService;
        readonly IMediator _mediator;



        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository,
            IWebHostEnvironment webHostEnvironment, IFileReadRepository fileReadRepository,
            IFileWriteRepositpory fileWriteRepositpory, IInvoiceFileReadRepository invoiceFileReadRepository,
            IInvoiceFileWriteRepository invoiceFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService, IConfiguration configuration, IMediator mediator, IProductService productService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepositpory = fileWriteRepositpory;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
            this.configuration = configuration;
            _mediator = mediator;
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
      {
          GetAllProductQueryResponse response= await  _mediator.Send(getAllProductQueryRequest);

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute]GetProductByIdQueryRequest getProductByIdQueryRequest)
        {
           GetProductByIdQueryResponse response=await _mediator.Send(getProductByIdQueryRequest);
            return Ok(response);
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Create Product")]
        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {
            CreateProductCommandResponse response=await _mediator.Send(createProductCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Update Product")]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]DeleteProductCommandRequest deleteProductCommandRequest)
        {
             DeleteProductCommandResponse response= await  _mediator.Send(deleteProductCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Writing, Definition = "Upload Product file ")]
        [HttpPost("[action]")]  //http://....../api/controller/action  //http://....../api/products/upload
        public async Task<IActionResult> upload([FromQuery]UploadProductImageCommandRequest uploadProductImageCommandRequest) ////..com/api/products?id=1 => id queryStringten gelir ,opsiyonel durumlarda kullanılır
        {
            uploadProductImageCommandRequest.files = Request.Form.Files;

            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);
            //string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "resource/product-images");
            //if (!Directory.Exists(uploadPath))
            //{
            //    Directory.CreateDirectory(uploadPath);
            //}
            //Random r = new();
            //foreach(var file in Request.Form.Files)
            //{
            //    string fullPath=Path.Combine(uploadPath, $"{r.NextDouble()}{Path.GetExtension(file.Name)}");
            //    using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
            //    await file.CopyToAsync(fileStream);
            //    await fileStream.FlushAsync();
            //}


            // forProduct
          //var datas=   await _storageService.UploadAsync("resource/files", Request.Form.Files);
          //  await _productImageFileWriteRepository.AddRangeAsync(datas.Select(c => new ProductImageFile()
          //  {
          //      FileName = c.fileName,
          //      Path = c.pathOrContainerName,
          //      width = 300,
          //      Storage=_storageService.StorageName
          //  }).ToList());
          //  await _productImageFileWriteRepository.SaveAsync();

            //InvoiceFile
            //await _invoiceFileWriteRepository.AddRangeAsync(datas.Select(c => new InvoiceFile()
            //{
            //    FileName = c.fileName,
            //    Path = c.path,
            //    Price=1200,
            //}).ToList());
            //await _invoiceFileWriteRepository.SaveAsync();

            //FileEntity
            //await _fileWriteRepositpory.AddRangeAsync(datas.Select(c => new FileEntity()
            //{
            //    FileName = c.fileName,
            //    Path = c.path,


            //}).ToList());
            //await _fileWriteRepositpory.SaveAsync();

            //var p = _productImageFileReadRepository.GetAll();
            //var I=_invoiceFileReadRepository.GetAll();
            //var file=_fileReadRepository.GetAll();
            return Ok();

        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Reading, Definition = "Get Product Images")]
        [HttpGet("[action]/{id}")]  //..com/api/products/1=>root datadan gelir , belli olan, opsiyon olmayan durunlarda kullanılır.
        public async Task<IActionResult> getProductImages([FromRoute]GetProductImageQueryRequest getProductImageQueryRequest)
        {
            List< GetProductImageQueryResponse> response = await _mediator.Send(getProductImageQueryRequest);
            return Ok( response);
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Deleting, Definition = "Delete Product Images")]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteProductImage([FromRoute]DeleteProductImageCommandRequest productImageCommandRequest, [FromQuery]string imageId)
        {
            productImageCommandRequest.imageId = imageId;
            DeleteProductImageCommandResponse response = await _mediator.Send(productImageCommandRequest);
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Products, ActionType = ActionType.Updating, Definition = "Change Showcase Image")]
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response =await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }


        [HttpGet("qrcode/{productId}")]
        public async Task<IActionResult> GetQrCodeToProduct([FromRoute] string productId)
        {
            var data = await _productService.QrCodeToProductAsync(productId);
            return File(data, "image/png");
        }



    }
}
