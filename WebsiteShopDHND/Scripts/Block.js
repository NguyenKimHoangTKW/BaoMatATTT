// chặn phím f12
document.addEventListener('keydown', function (event) {
    if (event.key === 'F12' || (event.ctrlKey && event.shiftKey && event.key === 'I')) {
        event.preventDefault();
    }
});

// chặn chuột phải
document.addEventListener('contextmenu', function (event) {
    event.preventDefault();
});

// chặn tổ hợp phím ctrl+shift+i (mở developer tools trên chrome)
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && event.shiftKey && event.key === 'I') {
        event.preventDefault();
    }
});

// chặn tổ hợp phím ctrl+shift+j (mở developer tools trên chrome)
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && event.shiftKey && event.key === 'J') {
        event.preventDefault();
    }
});

// chặn tổ hợp phím ctrl+shift+c (inspect element trên chrome)
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && event.shiftKey && event.key === 'C') {
        event.preventDefault();
    }
});

// chặn sao chép
document.addEventListener('copy', function (event) {
    event.preventDefault();
});

// chặn việc mở mã nguồn trang bằng tổ hợp phím ctrl+u
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && event.key === 'U') {
        event.preventDefault();
    }
});


function validateInput(input) {
    const scriptPattern = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;
    const apiPattern = /^(https?:\/\/)([^\/]+)/i;

    if (scriptPattern.test(input)) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Cảnh báo ! Bạn không được nhập dữ liệu dạng Script vào các ô nhập liệu',
        });
        return false;
    }

    // Kiểm tra xem input có chứa URL API không
    if (apiPattern.test(input)) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Cảnh báo ! Bạn không được nhập dữ liệu từ API bên ngoài',
        });
        return false;
    }

    return true;
};

// Hàm kiểm tra toàn bộ biểu mẫu trước khi gửi
function validateForm() {
    const inputElements = document.querySelectorAll('input, textarea');
    for (let inputElement of inputElements) {
        if (!validateInput(inputElement.value)) {
            return false;
        }
    }
    return true;
};

document.addEventListener("DOMContentLoaded", () => {
    const inputs = document.querySelectorAll("input, textarea");
    for (let input of inputs) {
        input.addEventListener("input", (event) => {
            const value = event.target.value;
            if (!validateInput(value)) {
                event.target.value = '';
            } else if (!validateAPICall(value)) {
                event.target.value = value.replace(/^(https?:\/\/)([^\/]+)/i, "");
            } else {
                event.target.value = value.replace(/<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi, "");
            }
        });
    }
});

function validateAPICall(input) {
    const apiPattern = /^(https?:\/\/)([^\/]+)/i;
    return !apiPattern.test(input);
}
