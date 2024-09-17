import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import LogsView from './components/logs/LogsView';
import MonitorsView from './components/monitors/MonitorsView';
import AlertsView from './components/alerts/AlertsView';
import './App.css';

const App = () => {
    return (
        <Layout>
            <Routes>
                <Route path="/alerts" element={<AlertsView />} />
                <Route path="/monitors" element={<MonitorsView />} />
                <Route path="/logs" element={<LogsView />} />
                <Route path="/" element={<LogsView />} />
            </Routes>
        </Layout>
    );
};

export default App;