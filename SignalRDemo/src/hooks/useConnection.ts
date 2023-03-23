import { HubConnectionBuilder, LogLevel, HubConnection } from '@microsoft/signalr'
import { useEffect, useState } from 'react'

import { TMessage, Source } from '../components/chat'

export const useConnection = (hubUrl: string, userId: string, stream: boolean = false) =>
{
    if (!hubUrl) return null

    const [connection, setConnection] = useState<HubConnection | null>(null)
    const [messages, setMessages] = useState<TMessage[]>([])

    const sendMessage = (message: string) =>
    {
        if (connection === null) return

        connection.invoke('Send', userId, message, stream)

        const userMessage: TMessage = {
            timestamp: new Date(),
            content: message,
            source: Source.USER
        }

        if(stream)
        {
            const blankResponse: TMessage = {
                timestamp: new Date(),
                content: '',
                source: Source.CHAT
            }

            setMessages(prevMessages => [...prevMessages, userMessage, blankResponse])
        }
        else
        {
            setMessages(prevMessages => [...prevMessages, userMessage])
        }
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
                setMessages(prevMessages => 
                    {
                        const newMessages = stream ? prevMessages.slice(0, -1) : prevMessages
                        
                        return [...newMessages, message]
                    })
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
