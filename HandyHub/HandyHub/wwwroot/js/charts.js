document.addEventListener("DOMContentLoaded", function () {
    const ctx = document.getElementById("chartOverview");
    if (!ctx) return;

    new Chart(ctx, {
        type: "bar",
        data: {
            labels: ["يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو"],
            datasets: [{
                label: "عدد المستخدمين المسجلين",
                data: [5, 9, 7, 12, 11, 15],
                backgroundColor: "rgba(0, 123, 255, 0.6)",
                borderColor: "#007bff",
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
});