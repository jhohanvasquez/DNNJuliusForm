//var edeqApp = angular.module("edeqApp", []);
CustomModule.controller("DNNJuliusFormController", function ($scope, $http) {

    $scope.model = { Respuesta: "true" };
    $scope.myFiles = [];
    $scope.error = false;
    $scope.echo = false;
    $scope.mensaje = '';
    $scope.titulo = '';

    //Se crea el acceso al controlador
    var $self = this;
    if ($.ServicesFramework) {
        var _sf = $.ServicesFramework(DNNJuliusFormId);
        $self.ServiceRoot = _sf.getServiceRoot('DNNJuliusForm');
        $self.ServicePath = $self.ServiceRoot + "Item/";
        $self.Headers = {
            "ModuleId": _sf.getModuleId(),
            "TabId": _sf.getTabId(),
            "RequestVerificationToken": _sf.getAntiForgeryValue()
        };
    }

    $scope.get = function () {
        $http({
            method: "GET",
            url: $self.ServicePath + 'Get',
            headers: $self.Headers
        }).then(function (response) {
            if (response.data) {
                $scope.departaments = response.data.Departament;
                $scope.cities = response.data.Cities;
            }
        },
            function (response) {
                switch (response.status) {
                    case 401:
                        $scope.showLogin = true;
                        $scope.continue = false;
                        break;
                    case 400:
                        $scope.showAlert = true;
                        $scope.typeAlert = 'error';
                        $scope.messageAlert = 'Error, por favor inténtelo de nuevo';
                        break;
                    case 500:
                        $scope.showAlert = true;
                        $scope.typeAlert = 'error';
                        $scope.messageAlert = 'Error, por favor inténtelo de nuevo';
                        break;
                    case 300:
                        $scope.showAlert = true;
                        $scope.typeAlert = 'error';
                        $scope.messageAlert = 'Ya existe una causa con ese nombre';
                        break;
                    default:
                }
            });
    }
    $scope.get();

    $scope.currentCity;
    $scope.loadCity = function (e, id) {
        $scope.currentCity = e;
        $http({
            method: "GET",
            url: $self.ServicePath + 'GetCityByDepartament?id=' + id,
            headers: $self.Headers
        }).then(function (response) {
            if (response.data) {
                switch ($scope.currentCity) {
                    case 's':
                        $scope.cities = response.data;
                        break;
                    case 'p':
                        $scope.citiesp = response.data;
                        break;
                    case 'quindio':
                        $scope.cities = response.data;
                        $scope.citiesp = response.data;
                        break;
                    default:
                }
            }
        }).catch(function (error) {
            console.log('Error: ', error);
        })
    }

    $scope.consultarSac = function () {

    }

    function getBase64(file) {

        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });
    }

    $scope.loadFile = function () {
        var files = $('input[type=file]');
        files.each(function (i, a) {

            var reader = new FileReader();
            reader.readAsDataURL(a);

            reader.onload = function () {
                console.log(reader.result);//base64encoded string
            };
            reader.onerror = function (error) {
                console.log('Error: ', error);
            };
        });
    }

    $scope.deleteFile = function (e) {
        var index = $scope.myFiles.indexOf(e);
        $scope.myFiles.splice(index, 1);
    }

    $scope.enviar = function () {
        //$scope.loadFile();
        $scope.showErrorUploadEmpty = false;
        var memoriasOk = $.grep($scope.myFiles, function (a) {
            return a.Source == 'planos'
        });

        var planosOk = $.grep($scope.myFiles, function (a) {
            return a.Source == 'memorias'
        });

        if (memoriasOk.length == 0 || planosOk.length == 0) {
            $scope.showErrorUploadEmpty = true;
        }

        if (pageIsValid('FormSolicitud') && !$scope.showErrorUploadEmpty) {
            showLoading();
            $scope.model.Files = $scope.myFiles;

            $http({
                method: "POST",
                url: $self.ServicePath + 'Add',
                headers: $self.Headers,
                data: $scope.model
            }).then(function (response) {
                hideLoading();
                if (response.status === 200) {
                    $scope.name = $scope.model.NombreRazonSocial;                   
                    $scope.myFiles = [];
                    Alerta('success', "Le informamos que su solicitud ha sido correctamente enviada,  le invitamos a revisar su correo electronico a donde llegará la confirmación de la solicitud e información adicional respecto al curso del tramite.  Gracias por  utilizar nuestros servicios en linea.", $scope.name);
                    //showAlert('Información procesada con éxito', 'success');
                }
            },
                function (response) {
                    hideLoading();
                    switch (response.status) {
                        case 401:
                            $scope.showLogin = true;
                            $scope.continue = false;
                            break;
                        case 400:
                            $scope.showAlert = true;
                            $scope.typeAlert = 'error';
                            $scope.messageAlert = 'Error, por favor inténtelo de nuevo';
                            break;
                        case 500:
                            showAlert(response.data, 'error');
                            break;
                        case 300:
                            $scope.showAlert = true;
                            $scope.typeAlert = 'error';
                            $scope.messageAlert = 'Ya existe una causa con ese nombre';
                            break;
                        default:
                    }
                });
        }
    }

    $scope.Redirect = function () {
        window.location.href = "/";
    }

    var Alerta = function (tipo, mensaje, titulo = '') {
        if (tipo == 'error') {
            $scope.error = true;
            $scope.titulo = 'Error!'
            $timeout(function () {
                $scope.error = false;
                $scope.echo = false;
            }, 3000);
        } else {
            $scope.echo = true;
            $scope.titulo = 'Sr/Sra ' + titulo;
        }
        $scope.mensaje = mensaje;
    }

    $scope.closeModal = function () {
        $scope.echo = false;
        $scope.error = false;
    }

});

CustomModule.directive('jsFileUpload', ['$parse', function ($parse) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var model = $parse(attrs.ngFileModel);
            element.bind('change', function () {
                var values = [];
                angular.forEach(element[0].files, function (item) {

                    if (!canUploadFile($(element[0]), scope)) { element[0].value = ""; return; }

                    var base64 = '';
                    let fileName = item.name;
                    getBase64(item).then(
                        data => {
                            scope.myFiles.push({ Base: data, Source: $(element[0]).data().fuente, fileName: fileName, Size: item.size }); scope.$apply();
                        }
                    );
                    var value = {
                        // File Name 
                        name: item.name,
                        //File Size 
                        size: item.size,
                        //File URL to view 
                        url: URL.createObjectURL(item),
                        // File Input Value 
                        _file: item
                    };
                    values.push(value);
                    element[0].value = ""
                });
            });
        }
    };
}]);

canUploadFile = function (e, scope) {
    var count = $.grep(scope.myFiles, function (a) {
        return a.Source == e.data().fuente
    });

    let maxSize = 0;
    let FileType = 0;
    if (count) {
        switch (e.data().fuente) {

            case 'planos':
                maxSize = 15728640;
                FileType = 15;
                break;
            case 'memorias':
                maxSize = 10485760;
                FileType = 10;
                break;
            case 'licencia':
                maxSize = 1048576
                FileType = 1;
                break;
            case 'permiso':
                maxSize = 1048576
                FileType = 1;
                break;
        }
    }

    let sumSize = e[0].files[0].size;
    count.forEach(function (file, i) {
        sumSize = sumSize + file.Size;
    });

    if (count.length >= e.data().maxFile) {
        showAlert('No se puede adjuntar el archivo, ha superado el límite de ' + $(e).data().maxFile, 'error');
        return false;
    }

    if (sumSize > maxSize) {
        showAlert(`Error adjuntando archivos, ha superado el máximo de peso para ${e.data().fuente}. Por favor ajunte varios archivos o un comprimido cuyo peso no supere la(s) ${FileType} mega(s)`, 'error');
        return false;
    }
    return true;
}


showAlert = function (message, type) {
    let alertExist = document.querySelector('.custom-alert');
    if (alertExist) {
        alertExist.parentNode.removeChild(alertExist);
    }

    let body = document.querySelector('body');
    let html = document.createElement('div');
    html.setAttribute('class', 'custom-alert ' + type);
    html.innerHTML = message
    body.appendChild(html);

    setTimeout(function () {
        let alertExist = document.querySelector('.custom-alert');
        if (alertExist) {
            alertExist.parentNode.removeChild(alertExist);
        }
    }, 10000);
}
function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

