$(document).ready(function () {
    ShowCount();
    $('body').on('click', '.btnAddToCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var tQuantity = $('#quantity_value').text();
        var quantity = 1;
        if (tQuantity != '')
        {
            quantity = parseInt(tQuantity);
        }
        $.ajax({
            url: '/ShoppingCart/AddToCart',
            type: 'POST',
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.success) {
                    $('#checkout_items').html(rs.count);
                    alert("Thêm sản phẩm vào giỏ hàng thành công");
                }
            }
        });
    });

    $('body').on('click', '.btnUpdate', function (e) {
        e.preventDefault();
        var id = $(this).data("id");
        var quantity = $('#Quantity_' + id).val();
        Update(id, quantity);
    });

    $('body').on('click', '.btnDeleteAll', function (e) {
        e.preventDefault();
        var conf = confirm("Bạn có chắc muốn xóa hết sản phẩm trong giỏ hàng");
        if (conf == true) {
            DeleteAll();
        }
    });

    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var conf = confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng");
        if (conf) {
            $.ajax({
                url: '/ShoppingCart/Delete',
                type: 'POST',
                data: { id: id },
                success: function (rs) {
                    if (rs.success) {
                        $('#checkout_items').html(rs.count);
                        $('#trow_' + id).remove();
                        LoadCart();
                    }
                }
            });
        }
    });
});

function ShowCount() {
    $.ajax({
        url: '/ShoppingCart/ShowCount',
        type: 'GET',
        success: function (rs) {
            $('#checkout_items').html(rs.count);
        }
    });
}

function Update(id,quantity) {
    $.ajax({
        url: '/ShoppingCart/Update',
        type: 'POST',
        data: { id: id, quantity: quantity },
        success: function (rs) {
            if (rs.success) {
                LoadCart();
            }
        }
    });
}

function DeleteAll() {
    $.ajax({
        url: '/ShoppingCart/DeleteAll',
        type: 'POST',
        success: function (rs) {
            if (rs.success) {
                LoadCart();
            }
        }
    });
}

function LoadCart() {
    $.ajax({
        url: '/ShoppingCart/Partial_Item_Cart',
        type: 'GET',
        success: function (rs) {
            $('#load_data').html(rs);
        }
    });
}