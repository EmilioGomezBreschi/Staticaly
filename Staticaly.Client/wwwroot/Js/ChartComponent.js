window.renderChart = (canvasId, chartType, labels, data) => {
    var ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, {
        type: chartType,
        data: {
            labels: labels,
            datasets: [{
                label: 'Datos',
                data: data,
                backgroundColor: '#df9ea754',
                borderColor: '#87493f',
                borderWidth: 1
            }]
        },
        options: {
          responsive: true,
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
};
