import React, { useState, useEffect } from 'react';
import axios from 'axios';
import LogChart from './LogChart';

const LogsView = () => {
    const [logs, setLogs] = useState([]);
    const [totalLogs, setTotalLogs] = useState(0);
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [filters, setFilters] = useState({ ipAddress: '', applicationName: '' });

    useEffect(() => {
        fetchLogs();
    }, [page, pageSize, filters]);

    const fetchLogs = async () => {
        try {
            console.log('Fetching logs with params:', {
                ipAddress: filters.ipAddress,
                applicationName: filters.applicationName,
                page: page,
                pageSize: pageSize
            });
            const response = await axios.get('/api/LogEntry', {
                params: {
                    ipAddress: filters.ipAddress,
                    applicationName: filters.applicationName,
                    page: page,
                    pageSize: pageSize
                }
            });
            console.log('Response:', response.data);
            setLogs(response.data.logs || []);
            setTotalLogs(response.data.totalLogs || 0);
        } catch (error) {
            console.error('Error fetching logs:', error);
        }
    };

    const handleFilterChange = (e) => {
        setFilters({ ...filters, [e.target.name]: e.target.value });
    };

    const handlePageChange = (newPage) => {
        setPage(newPage);
    };

    console.log("Logs data:", logs);

    return (
        <div>
            <h2>Logs</h2>
            <div>
                <input
                    type="text"
                    name="ipAddress"
                    placeholder="IP Address"
                    value={filters.ipAddress}
                    onChange={handleFilterChange}
                />
                <input
                    type="text"
                    name="applicationName"
                    placeholder="Application Name"
                    value={filters.applicationName}
                    onChange={handleFilterChange}
                />
            </div>
            <LogChart data={logs} />
            <table className="table">
                <thead>
                    <tr>
                        <th>Timestamp</th>
                        <th>Device ID</th>
                        <th>Application Name</th>
                        <th>IP Address</th>
                        <th>Log Message</th>
                    </tr>
                </thead>
                <tbody>
                    {logs.map(log => (
                        <tr key={log.id}>
                            <td>{log.timestamp}</td>
                            <td>{log.deviceId}</td>
                            <td>{log.applicationName}</td>
                            <td>{log.ipAddress}</td>
                            <td>{log.logMessage}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
            <div>
                <button onClick={() => handlePageChange(page - 1)} disabled={page === 1}>Previous</button>
                <button onClick={() => handlePageChange(page + 1)} disabled={page * pageSize >= totalLogs}>Next</button>
            </div>
        </div>
    );
};

export default LogsView;
