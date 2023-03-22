import { HubConnectionBuilder, LogLevel, HubConnection } from '@microsoft/signalr'
import { useEffect, useState } from 'react'

import { TMessage, Source } from '../components/chat'

export const useConnection = (hubUrl: string, userId: string) =>
{
    if (!hubUrl) return null

    const [connection, setConnection] = useState<HubConnection>(null)
    const [messages, setMessages] = useState<TMessage[]>([])

    const sendMessage = (message: string) =>
    {
        if (connection === null) return

        connection.invoke('Send', userId, message)

        const userMessage: TMessage = {
            timestamp: new Date(),
            content: message,
            source: Source.USER
        }

        setMessages(prevMessages => [...prevMessages, userMessage])
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

            conn.invoke('Register', userId)

            conn.on('Response', (message: TMessage) => 
            {
                setMessages(prevMessages => [...prevMessages, message])
            })

            conn.on('Registered', (messages: TMessage[]) =>
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
