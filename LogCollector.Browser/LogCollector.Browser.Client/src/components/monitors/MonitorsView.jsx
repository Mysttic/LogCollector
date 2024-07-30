import React, { useState, useEffect } from 'react';
import axios from 'axios';
import MonitorDetails from './MonitorDetails';
import './MonitorsView.css';

const MonitorsView = () => {
    const [monitors, setMonitors] = useState([]);
    const [totalMonitors, setTotalMonitors] = useState(0);
    const [selectedMonitor, setSelectedMonitor] = useState(null);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);

    useEffect(() => {
        fetchMonitors();
    }, [page, pageSize]);

    const fetchMonitors = async () => {
        try {
            const response = await axios.get('/api/Monitor', {
                params: {
                    pageNumber: page,
                    pageSize: pageSize,
                    startIndex: ((page - 1) * pageSize)
                }
            });
            console.log('Response:', response.data);
            setMonitors(response.data.pagedMonitorResult.items || []);
            setTotalMonitors(response.data.totalCount || 0);
        } catch (error) {
            console.error('Error fetching monitors:', error);
        }
    };

    const handleSelect = (monitor) => {
        setSelectedMonitor(monitor);
        };

    const handlePageChange = (newPage) => {
        setPage(newPage);
    };

    console.log("Monitors data:", monitors);

    return (
        <div>
            <h2>Monitors</h2>
            <div className="monitor-container">
                <table className="monitor-table">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Action</th>
                            <th>Alerts</th>
                        </tr>
                    </thead>
                    <tbody>
                        {monitors.map((monitor) => (
                            <tr key={monitor.id} onClick={() => handleSelect(monitor)}>
                                <td>{monitor.name}</td>
                                <td>{monitor.action}</td>
                                <td>{monitor.alerts.length}</td>
                            </tr>
                        ))}
                    </tbody>
                        </table>
                <div className="monitor-details">
                    {selectedMonitor && (
                        <MonitorDetails monitor={selectedMonitor} onClose={() => setSelectedMonitor(null)} />
                    )}
                </div>
            </div>
        </div>
    );
};

export default MonitorsView;
