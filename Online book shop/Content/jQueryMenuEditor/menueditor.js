 jQuery(document).ready(function () {
            /* =============== DEMO =============== */
            // menu items
            var arrayjson = $('#out').val();//[{ "href": "http://home.com", "icon": "fas fa-home", "text": "Home", "target": "_top", "title": "My Home" }];//, { "icon": "fas fa-chart-bar", "text": "Opcion2" }, { "icon": "fas fa-bell", "text": "Opcion3" }, { "icon": "fas fa-crop", "text": "Opcion4" }, { "icon": "fas fa-flask", "text": "Opcion5" }, { "icon": "fas fa-map-marker", "text": "Opcion6" }, { "icon": "fas fa-search", "text": "Opcion7", "children": [{ "icon": "fas fa-plug", "text": "Opcion7-1", "children": [{ "icon": "fas fa-filter", "text": "Opcion7-1-1" }] }] }];
            // icon picker options
            var iconPickerOptions = { searchText: "Buscar...", labelHeader: "{0}/{1}" };
            // sortable list options
            var sortableListOptions = {
                placeholderCss: { 'background-color': "#cccccc" }
            };

            var editor = new MenuEditor('myEditor', { listOptions: sortableListOptions, iconPicker: iconPickerOptions });
            editor.setForm($('#frmEdit'));
            editor.setUpdateButton($('#btnUpdate'));
            editor.setData(arrayjson);
            $('#btnReload').on('click', function () {
                editor.setData(arrayjson);
            });

            $('#btnOutput').on('click', function () {
                var str = editor.getString();
                $("#out").text(str);
            });

            $("#btnUpdate").click(function () {
                editor.update();
                $("#btnOutput").trigger("click");
            });

            $('#btnAdd').click(function () {
                editor.add();
                $("#btnOutput").trigger("click");
            });

        });