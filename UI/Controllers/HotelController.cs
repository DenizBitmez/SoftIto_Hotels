using Business.Abstract;
using Business.Concreate;
using Data.Abstract;
using Data.Concreate;
using Data.EntityFramework;
using Entity.Concreate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HotelController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly Context _context;
        private readonly IOrderService _orderService;
        private readonly IHotelService _hotelService;
        //HotelManager _hotelManager = new HotelManager(new EFHotelDal());
        //CustomerManager _customerManager=new CustomerManager(new EFCustomerDal(),new Context());

        //public HotelController()
        //{
        //    _customerService = new CustomerManager(new EFCustomerDal(), new Context());
        //    _context = new Context();
        //}


        public HotelController(ICustomerService customerService, Context context,IOrderService orderService, IHotelService hotelService)
        {
            _customerService = customerService;
            _context = context;
            _orderService = orderService;
            _hotelService = hotelService;
        }
        public HotelController()
        {
            _customerService = new CustomerManager(new EFCustomerDal(), new Context());
            _context = new Context();
            _orderService = new OrderManager(new EFOrderDal());
            _hotelService = new HotelManager(new EFHotelDal());
        }

        // GET: Hotel
        public ActionResult Index()
        {
            var sonuc=_hotelService.Hotelliste();
            return View(sonuc);
        }

        public ActionResult Booking()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Hotel");
            }
            int userId = Convert.ToInt32(Session["UserID"]);
            var hotels = _hotelService.Hotelliste(); 

            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelTopic");
            var customerBooking = _orderService.GetById(userId) ?? new Order();

            return View(customerBooking);

        }

        [HttpPost]
        public ActionResult Booking(Order order)
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Hotel");
            }

            if (ModelState.IsValid)
            {
                order.CustomerId = Convert.ToInt32(Session["UserID"]);
                _orderService.OrderInsert(order);
                return RedirectToAction("Booking");
            }
            var hotels = _hotelService.Hotelliste();
            ViewBag.Hotels = new SelectList(hotels, "HotelId", "HotelTopic");
            return View(order);
        }
        public ActionResult Amenities()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Rooms()
        {
            var sonuc = _hotelService.Hotelliste();
            return View(sonuc);
        }

        public ActionResult Contact()
        {
            return View();
        }

        //public ActionResult Login()
        //{
        //    List<SelectListItem> sonuc = (from x in cm.Customerliste()
        //                                  select new SelectListItem
        //                                  {
        //                                      Text = x.CustomerId.ToString(),
        //                                      Value = x.OrderId.ToString()
        //                                  }).ToList();
        //    ViewBag.d = sonuc;
        //    return View();
        //}

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Lütfen tüm alanları eksiksiz doldurun!";
                return View();
            }
            var existingUser = _customerService.Customerliste().FirstOrDefault(x => x.Username == customer.Username);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Bu kullanıcı adı zaten kullanılıyor!";
                return View();
            }

            // Yeni kullanıcıyı ekle
            _customerService.CustomerInsert(customer);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Login", "Hotel");
        }
 
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "Hotel");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Username, string Password)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ViewBag.ErrorMessage = "Lütfen kullanıcı adı ve şifre giriniz!";
                return View();
            }

            var user = _customerService.Customerliste().FirstOrDefault(c => c.Username == Username && c.Password == Password);

            if (user != null)
            {
                Session["UserID"] = user.CustomerId;
                Session["Username"] = user.Username;
                Session["IsAdmin"] = user.IsAdmin;

                if (user.IsAdmin)
                {
                    return RedirectToAction("Dashboard", "Hotel");
                }
                else 
                {
                    return RedirectToAction("Index", "Hotel");
                }
            }

            ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
            return View("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        private List<Customer> GetCustomers()
        {
            return new List<Customer>
            {
                new Customer { CustomerId = 1, Username = "admin", Password = "admin123", IsAdmin = true },
                new Customer { CustomerId = 2, Username = "user1", Password = "password1", IsAdmin = false },
                new Customer { CustomerId = 3, Username = "user2", Password = "password2", IsAdmin = false }
            };
        }

        public ActionResult Dashboard()
        {
            if (Session["UserID"] == null || Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                ViewBag.ErrorMessage = "Bu sayfaya yalnızca adminler erişebilir!";
                return RedirectToAction("Login", "Hotel");
            }

            return View();
        }

        public ActionResult HotelAdd()
        {
            return View(new Hotel());
        }

        [HttpPost]
        public ActionResult HotelAdd(Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _hotelService.HotelInsert(hotel);
                return RedirectToAction("HotelList");
            }
            return View(hotel);
        }

    }
}