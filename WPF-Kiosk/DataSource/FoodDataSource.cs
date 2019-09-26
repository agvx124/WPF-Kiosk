using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Kiosk.Common;
using WPF_Kiosk.Model;

namespace WPF_Kiosk
{
    public class FoodDataSource
    {
        bool isLoaded = false;
        public List<Food> listFood = null;

        public void Load()
        {
            if (isLoaded) return;

            listFood = new List<Food>()
            {
                // 커피
                new Food() {Category = eCategory.Coffee, Name = "더블에스프레소", Price = 2000, ImagePath = @"/Assets\Coffee\더블에스프레소.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "앗!메리카노", Price = 2000, ImagePath = @"/Assets\Coffee\앗!메리카노.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "원조커피", Price = 2500, ImagePath = @"/Assets\Coffee\원조커피.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "달달연유라떼", Price = 2500, ImagePath = @"/Assets\Coffee\달달연유라떼.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "빽's 라떼", Price = 3000, ImagePath = @"/Assets\Coffee\백스라떼.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "블랙펄카페라떼", Price = 2500, ImagePath = @"/Assets\Coffee\블랙펄카페라떼.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "바닐라라떼", Price = 3500, ImagePath = @"/Assets\Coffee\바닐라라떼.jpg"},
                new Food() {Category = eCategory.Coffee, Name = "카페모카", Price = 4000, ImagePath = @"/Assets\Coffee\카페모카.jpg"},

                // 음료
                new Food() {Category = eCategory.Drink, Name = "완전초코", Price = 3500, ImagePath = @"/Assets\Drink\완전초코.jpg"},
                new Food() {Category = eCategory.Drink, Name = "녹차라떼", Price = 3000, ImagePath = @"/Assets\Drink\녹차라떼.jpg"},
                new Food() {Category = eCategory.Drink, Name = "토피넛라떼", Price = 3000, ImagePath = @"/Assets\Drink\토피넛라떼.jpg"},
                new Food() {Category = eCategory.Drink, Name = "민트초코라떼", Price = 3500, ImagePath = @"/Assets\Drink\민트초코라떼.jpg"},
                new Food() {Category = eCategory.Drink, Name = "밀크티", Price = 3000, ImagePath = @"/Assets\Drink\밀크티.jpg"},
                new Food() {Category = eCategory.Drink, Name = "블랙펄라떼", Price = 2500, ImagePath = @"/Assets\Drink\블랙펄라떼.jpg"},
                new Food() {Category = eCategory.Drink, Name = "블랙펄밀크티", Price = 2500, ImagePath = @"/Assets\Drink\블랙펄밀크티.jpg"},
                new Food() {Category = eCategory.Drink, Name = "깔라만시티", Price = 2000, ImagePath = @"/Assets\Drink\깔라만시티.jpg"},

                // 아이스크림/디저트
                new Food() {Category = eCategory.Desert, Name = "노말한소프트", Price = 2000, ImagePath = @"/Assets\Desert\노말한소프트.jpg"},
                new Food() {Category = eCategory.Desert, Name = "옥수크림", Price = 2500, ImagePath = @"/Assets\Desert\옥수크림.jpg"},
                new Food() {Category = eCategory.Desert, Name = "호두크런치", Price = 2500, ImagePath = @"/Assets\Desert\호두크런치.jpg"},
                new Food() {Category = eCategory.Desert, Name = "빽엔나", Price = 3000, ImagePath = @"/Assets\Desert\빽엔나.jpg"},
                new Food() {Category = eCategory.Desert, Name = "사라다빵", Price = 2000, ImagePath = @"/Assets\Desert\사라다빵.jpg"},
                new Food() {Category = eCategory.Desert, Name = "소세지빵", Price = 2500, ImagePath = @"/Assets\Desert\소세지빵.jpg"},
                new Food() {Category = eCategory.Desert, Name = "크리미슈", Price = 2000, ImagePath = @"/Assets\Desert\크리미슈.jpg"},
                new Food() {Category = eCategory.Desert, Name = "크리미단팥빵", Price = 2500, ImagePath = @"/Assets\Desert\크리미단팥빵.jpg"},

                // 빽스치노(시그니처메뉴)
                new Food() {Category = eCategory.SignatureMenu, Name = "카라멜 빽스치노", Price = 4500, ImagePath = @"/Assets\SignatureMenu\카라멜빽스치노.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "베리크런치 빽스치노", Price = 4500, ImagePath = @"/Assets\SignatureMenu\베리크런치빽스치노.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "완전딸기바나나", Price = 3500, ImagePath = @"/Assets\SignatureMenu\완전딸바.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "완전초코바나나", Price = 3500, ImagePath = @"/Assets\SignatureMenu\완전초바.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "피스타치오빽스치노", Price = 4500, ImagePath = @"/Assets\SignatureMenu\피스타치오빽스치노.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "녹차빽스치노", Price = 4000, ImagePath = @"/Assets\SignatureMenu\녹차빽스치노.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "원조빽스치노", Price = 4000, ImagePath = @"/Assets\SignatureMenu\원조빽스치노.jpg"},
                new Food() {Category = eCategory.SignatureMenu, Name = "민트초코 빽스치노", Price = 4000, ImagePath = @"/Assets\SignatureMenu\민트초코빽스치노.jpg"},

            };
        }
    }
}
