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

// chặn việc mở mã nguồn trang bằng tổ hợp phím ctrl+u
document.addEventListener('keydown', function (event) {
    if (event.ctrlKey && event.key === 'U') {
        event.preventDefault();
    }
});

