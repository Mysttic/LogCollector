import React, { useRef, useEffect } from 'react';
import { Chart, CategoryScale, LinearScale, BarElement, BarController, Title, Tooltip, Legend } from 'chart.js';

Chart.register(CategoryScale, LinearScale, BarElement, BarController, Title, Tooltip, Legend);

const LogChart = ({ data }) => {
    const chartRef = useRef(null);

    useEffect(() => {
        if (chartRef && chartRef.current && data && data.length > 0) {
            const ctx = chartRef.current.getContext('2d');
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: data.map(log => log.applicationName),
                    datasets: [
                        {
                            label: 'Logs',
                            data: data.map(log => log.logMessage.length),
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            borderColor: 'rgba(75, 192, 192, 1)',
                            borderWidth: 1
                        }
                    ]
                },
                options: {
                    scales: {
                        x: {
                            type: 'category',
                            labels: data.map(log => log.applicationName)
                        },
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    }, [data]);

    if (!data || data.length === 0) {
        return <div>No data available</div>;
    }

    return <canvas ref={chartRef} />;
};

export default LogChart;
