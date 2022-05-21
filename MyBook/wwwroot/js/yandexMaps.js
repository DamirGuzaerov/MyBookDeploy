ymaps.ready(init);

async function init(){
    var myMap = new ymaps.Map("map", {
        center: [55.76, 37.64],//Moscow
        zoom: 10
    });
    
    $.ajax(
        {
            type: "GET",
            url:`/Home/GetBooksCenter`,
            success: function (data) {
                $.each(data,function(index,value){
                    console.log(value)
                    myMap.geoObjects.add(new ymaps.Placemark([value.latitude, value.longitude], {
                        balloonContent: `Тут наш центр`
                    }, 
                        {
                        preset: 'islands#circleDotIcon',
                        iconColor: 'red'
                    }))
                })
            },
            error: function (data){
                alert("Что-то пошло не так при загрузке карты , повторите попытку позже.");
            }
        }
    )
}