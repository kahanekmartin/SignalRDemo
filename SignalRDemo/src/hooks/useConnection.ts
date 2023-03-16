import { HubConnectionBuilder, LogLevel, HubConnection } from '@microsoft/signalr'
import { useEffect, useState } from 'react'

import { TMessage } from '../components/chat'

export const useConnection = (hubUrl: string) =>
{
    if (!hubUrl) return null

    const [connection, setConnection] = useState<HubConnection>()
    const [messages, setMessages] = useState([])

    const sendMessage = (userId: string, message: string) =>
    {
        if (connection === null) return

        connection.invoke('Send', userId, message)
    }

    const initCommunication = () =>
    {
        const conn = (new HubConnectionBuilder())
            .withUrl(hubUrl)
            .withAutomaticReconnect([0, 3000, 8000, 14000])
            .configureLogging(LogLevel.Information)
            .build()

        conn.start().then(() =>
        {
            setConnection(conn)

            conn.on('Receive', (messages: TMessage[]) =>
            {
                setMessages(messages)
            })
        })
    }

    useEffect(() =>
    {
        if (connection === null)
        {
            initCommunication()
        }
    }, [])

    return {
        sendMessage,
        messages
    }
}
