// رسائل Toast عبر TempData
function showToast(type, message) {
    const toast = document.createElement("div");
    toast.className = `toast align-items-center text-white border-0 bg-${type} position-fixed top-0 end-0 m-3 fade show`;
    toast.style.zIndex = 2000;
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">${message}</div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>`;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 3000);
}

// زر الرجوع للأعلى
window.addEventListener("scroll", () => {
    const btn = document.getElementById("backToTop");
    btn.style.display = window.scrollY > 200 ? "block" : "none";
});