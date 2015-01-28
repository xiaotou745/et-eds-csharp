yg.data.usa.getProvinces = function () {
    return yg.data.usa.provinces;
}

yg.data.usa.getCities = function (provinceId) {
	var result = yg.data.usa.cities[provinceId];
	if (!result) {
		return [];
	}
	return result;
}

yg.data.usa.getAddress = function (provinceId, cityId) {
	var address = "";

	var cities = us.data.usa.getCities(provinceId);
	for (var i = 0; i < cities.length; i++) {
		if (cities[i].Value == cityId) {
			address += cities[i].Text + " , ";
			break;
		}
	}

	for (var i = 0; i < art.data.china.provinces.length; i++) {
		if (art.data.china.provinces[i].Value == provinceId) {
			address += art.data.china.provinces[i].Text;
			break;
		}
	}
 
	return address;
}

yg.data.usa.initAreaSelect = function(provinceitem, cityitem){
    var province = $(provinceitem);
    $(province).html("");
    var pro_data = yg.data.usa.getProvinces();
    for (var i = 0; i < pro_data.length; i++) {
        $("<option value='" + pro_data[i].Value + "'>" + pro_data[i].Text + "</option>").appendTo(province);
    }
    var city = $(cityitem);
    province.change(function () {
        $(cityitem).html("");
        var pro_id = $(this).val();

        var city_data = yg.data.usa.getCities(pro_id);
        for (var i = 0; i < city_data.length; i++) {
            $("<option value='" + city_data[i].Value + "'>" + city_data[i].Text + "</option>").appendTo(cityitem);
        }

    });
}
