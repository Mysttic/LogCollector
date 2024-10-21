import { fileURLToPath, URL } from 'node:url';
import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import child_process from 'child_process';
import { env } from 'process';

// Sprawdzenie, czy działamy w środowisku Docker
const isDocker = env.DOCKER === 'true';

const baseFolder =
    env.APPDATA !== undefined && env.APPDATA !== ''
        ? `${env.APPDATA}/ASP.NET/https`
        : `${env.HOME}/.aspnet/https`;

const certificateName = "logcollector.browser.client";
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

if (!isDocker && (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath))) {
    if (0 !== child_process.spawnSync('dotnet', [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
    ], { stdio: 'inherit', }).status) {
        throw new Error("Could not create certificate.");
    }
}

// httpsConfig zawiera HTTPS tylko wtedy, gdy nie działa w Dockerze
const httpsConfig = !isDocker
    ? {
        key: fs.readFileSync(keyFilePath),
        cert: fs.readFileSync(certFilePath)
    }
    : false;

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    // Ustal docelowy serwer API i port na podstawie trybu (development lub production)
    let target = 'https://localhost:7148';
    let port = 9084;

    if (mode === 'development') {
        target = 'https://localhost:7148'; // Adres backendu dla development
        port = 5173;
    } 

    return {
        plugins: [plugin()],
        resolve: {
            alias: {
                '@': fileURLToPath(new URL('./src', import.meta.url))
            }
        },
        server: {
            proxy: {
                '/api': {
                    target,
                    secure: false,
                    changeOrigin: true
                }
            },
            port: port,
            https: httpsConfig
        }
    };
});
