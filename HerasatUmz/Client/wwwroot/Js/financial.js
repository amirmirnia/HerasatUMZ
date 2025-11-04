function initializeCharts(data) {
    // Daily Revenue Chart
    const dailyCtx = document.getElementById('dailyRevenueChart').getContext('2d');
    new Chart(dailyCtx, {
        type: 'line',
        data: {
            labels: data.DailyRevenue.Labels,
            datasets: [{
                label: 'درآمد روزانه',
                data: data.DailyRevenue.Data,
                borderColor: '#4f46e5',
                backgroundColor: 'rgba(79, 70, 229, 0.2)',
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    title: { display: true, text: 'درآمد (تومان)' }
                },
                x: {
                    title: { display: true, text: 'تاریخ' }
                }
            }
        }
    });

    // Room Revenue Chart
    const roomCtx = document.getElementById('roomRevenueChart').getContext('2d');
    new Chart(roomCtx, {
        type: 'bar',
        data: {
            labels: data.RoomRevenue.Labels,
            datasets: [{
                label: 'درآمد اتاق',
                data: data.RoomRevenue.Data,
                backgroundColor: '#4f46e5',
                borderColor: '#4f46e5',
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    title: { display: true, text: 'درآمد (تومان)' }
                },
                x: {
                    title: { display: true, text: 'اتاق' }
                }
            }
        }
    });
}